using AutoMapper;
using IntraNet.Data.Context;
using IntraNet.Mod.SGR.Models.Identity;
using IntraNet.Security.ContextIdentity;
using IntraNet.Security.Identity;
using IntraNet.Security.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IntraNet.Mod.SGR.Controllers
{
    public class UserController : Controller
    {
        //operaçoes crud usuário interface
        public ApplicationDbContext db_ = new ApplicationDbContext();
        public ContextSGR db = new ContextSGR();


        public async Task<ActionResult> Dados(string id)
        {
            ContextRDV db = new ContextRDV();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);
            ViewBag.Claims = await UserManager.GetClaimsAsync(user.Id);
            ViewBag.Local = new SelectList(db.Empresa.ToList(), "EmpresaId", "RazaoSocial");
            ViewBag.Setor = new SelectList(db.Setor.ToList(), "SetorId", "Nome");
            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                //UnidadeId = user.UnidadeId,
                //SetorId = user.SetorId,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }
        public JsonResult GetUser(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var UserList = (from r in db_.Users.Include(x => x.Roles).AsEnumerable()
                join un in db_.Unidade on r.UnidadeId equals un.UnidadeId
                join t in db_.Setor on r.SetorId equals t.SetorId
                select new
                {
                    r.UserName,
                    r.Id,
                    r.UnidadeId,
                    Unidade = un.Nome ?? "-N/A-",
                    r.Matricula,
                    r.SetorId,
                    SetorNome = t.Nome ?? "-N/A-",
                    r.FullName,
                    //r.Endereco,
                    r.PhoneNumber,
                    Roles = RoleManager.Roles.ToList().Where(role=>role.Id == db_.Users.Where(u => u.Id == r.Id).Select(rol=>rol.Roles.FirstOrDefault().RoleId).FirstOrDefault()).Select(roles=>roles.Name).FirstOrDefault(),
                    r.Email
                });

            int totalRecords = UserList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

            if (sort.ToUpper() == "DESC")
            {
                UserList = UserList.OrderBy(t => t.UserName);
                UserList = UserList.Skip(pageIndex * pageSize).Take(pageSize);
            }

            else
            {
                UserList = UserList.OrderBy(t => t.UserName);
                UserList = UserList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = UserList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ActionResult Create()
        {
            
            ViewBag.Local = new SelectList(db_.Empresa.ToList(), "EmpresaId", "RazaoSocial");
            ViewBag.Unidade = new SelectList(db_.Unidade.ToList(), "UnidadeId", "Nome");
            ViewBag.SetorId = new SelectList(db_.Setor.ToList(), "SetorId", "Nome");
            ViewBag.FuncaoId = new SelectList(db_.Funcao.ToList(), "FuncaoId", "Nome");
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(db_.Roles.ToList(), "Name", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                
                var user = new ApplicationUser
                {
                    UserName = userViewModel.UserName,
                    FullName = userViewModel.FullName,
                    Email = userViewModel.Email,
                    UnidadeId = userViewModel.UnidadeId,
                    SetorId = userViewModel.SetorId,
                    PhoneNumber = userViewModel.PhoneNumber,
                    FuncaoId = userViewModel.SetorId,
                    Matricula = userViewModel.Matricula,
                    EmailConfirmed = true
                };
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);
                if (adminresult.Succeeded)
                {
                    if (userViewModel.Endereco != null)
                    {

                        userViewModel.Endereco.UserId = db_.Users.Where(u => u.UserName == userViewModel.UserName).Select(u => u.Id).SingleOrDefault();
                        EnderecoViewModel enderecoView = userViewModel.Endereco; 
                        Endereco enderecoDomain = Mapper.Map<EnderecoViewModel, Endereco>(enderecoView);
                       
                        db_.Endereco.Add(enderecoDomain);
                    }

                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }

                    db_.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View();
                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> CreateAdmin(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            string message = "";
            var errors = new List<string>();
            var success = 0;
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser
                {
                    UserName = userViewModel.UserName,
                    FullName = userViewModel.FullName,
                    Email = userViewModel.Email,
                    UnidadeId = userViewModel.UnidadeId,
                    SetorId = userViewModel.SetorId,
                    PhoneNumber = userViewModel.PhoneNumber,
                    FuncaoId = userViewModel.SetorId,
                    Matricula = userViewModel.Matricula,
                    EmailConfirmed = true
                };
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);
                if (adminresult.Succeeded)
                {
                    if (userViewModel.Endereco != null)
                    {
                        userViewModel.Endereco.UserId = db_.Users.Where(u => u.UserName == userViewModel.UserName).Select(u => u.Id).SingleOrDefault();
                        EnderecoViewModel enderecoView = userViewModel.Endereco;
                        Endereco enderecoDomain = Mapper.Map<EnderecoViewModel, Endereco>(enderecoView);
                        db_.Endereco.Add(enderecoDomain);
                    }

                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            //return View();
                        }
                    }
                    db_.SaveChanges();
                    message = "Cadastrado sucesso";
                    success = 1;
                }
            }
            else
            {
                foreach (var modelStateVal in ViewData.ModelState.Values)
                {
                    errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                }

                success = 0;
                return Json(new { success = success, message = errors }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = success, message = message }, JsonRequestBehavior.AllowGet);
            
        }
        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            
            var userRoles = await UserManager.GetRolesAsync(user.Id);
            //ViewBag.Claims = await UserManager.GetClaimsAsync(user.Id);
            ViewBag.UnidadeId = new SelectList(db_.Unidade.Where(u=>u.Ativo!=false).ToList(), "UnidadeId", "Nome", user.UnidadeId);
            ViewBag.SetorId = new SelectList(db_.Setor.Where(s=>s.Ativo!=false).ToList(), "SetorId", "Nome", user.SetorId);
            ViewBag.FuncaoId = new SelectList(db_.Funcao.Where(f=>f.Ativo!=false).ToList(), "FuncaoId", "Nome", user.FuncaoId);
            return View(new EditUserViewModel()
            {
                Id= user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                UnidadeId = user.UnidadeId,
                SetorId = user.SetorId,
                PhoneNumber = user.PhoneNumber,
                FuncaoId = user.SetorId,
                Matricula = user.Matricula,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EditAdmin(EditUserViewModel editUser, params string[] selectedRole)
        {
            string message = "";
            var errors = new List<string>();
            if (ModelState.IsValid)
            {
                var user =db_.Users.Find(editUser.Id);

                user.UserName = editUser.UserName;
                user.FullName = editUser.FullName;
                user.Email = editUser.Email;
                user.UnidadeId = editUser.UnidadeId;
                user.SetorId = editUser.SetorId;
                user.PhoneNumber = editUser.PhoneNumber;
                user.FuncaoId = editUser.FuncaoId;
                user.Matricula = editUser.Matricula;
                var userRoles = RoleManager.Roles.ToList()
                    .Where(role => role.Id == db_.Users.Where(u => u.Id == editUser.Id)
                                       .Select(rol => rol.Roles.FirstOrDefault().RoleId).FirstOrDefault())
                    .Select(roles => roles.Name);

                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    foreach (var modelStateVal in ViewData.ModelState.Values)
                    {
                        errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                    }
                    return Json(new { success = 0, message = errors });
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());
                if (!result.Succeeded)
                {
                    foreach (var modelStateVal in ViewData.ModelState.Values)
                    {
                        errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                    }
                    return Json(new { success = 0, message = errors });
                }
                db_.Users.AddOrUpdate(user);
                await db_.SaveChangesAsync();
                message = "Funcionário alterado com sucesso";
                return Json(new { success = 1, message = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                foreach (var modelStateVal in ViewData.ModelState.Values)
                {
                    errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                }
                return Json(new { success = 0, message = errors });
            }
            return Json(new { success = 0, message = errors }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost]
        public async Task<JsonResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return Json(new { success = 0, message = "Selecione um usuário" }, JsonRequestBehavior.AllowGet);
                }
                var user = await UserManager.FindByIdAsync(id);
                if (await UserManager.GetRolesAsync(id)!=null)
                {
                    var roles = await UserManager.GetRolesAsync(id);
                    await UserManager.RemoveFromRolesAsync(id, roles.ToArray());
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return Json(new { success = 0, message = "Não foi possivel remover o usuário" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = 1, message = "Usuário removido com sucesso" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = 0, message = "Não foi possivel remover o usuário" }, JsonRequestBehavior.AllowGet);
        }
    }
}