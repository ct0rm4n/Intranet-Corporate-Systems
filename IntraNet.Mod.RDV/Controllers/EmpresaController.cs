using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using IntraNet.Data.Context;
using IntraNet.Data.Repositories;
using IntraNet.Domain.Entities;
using IntraNet.Mod.RDV.Models.ViewModel;
using Newtonsoft.Json;

namespace IntraNet.Mod.RDV.Controllers
{
    [Authorize]
    public class EmpresaController : Controller
    {
        EmpresaRepository _repEmp = new EmpresaRepository();
        SetorRepository _repSetor = new SetorRepository();
        private EmpresaRepository _rep = new EmpresaRepository();
        // GET: Empresas/Details/5
        [HttpGet]
        public ActionResult ListaEmpresas()
        {
            List<Empresa> empresas = new List<Empresa>();
            foreach (var empresa in _rep.RecurperarTodos())
            {
                empresas.Add(empresa);
            }
            string values = JsonConvert.SerializeObject(empresas);
            //var json = new JavaScriptSerializer().Serialize(d);
            return Json(new { empresas }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int id)
        {
            Empresa empresa = _rep.RecuperarPorID(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }
        public ActionResult SelecionaEmpresa()
        {
            ViewBag.EmpresaId = new SelectList(_rep.GetAll(), "EmpresaId", "RazaoSocial");
            return View();
        }
        // GET: Empresas/Create
        public ActionResult Create()
        {
            ViewBag.SetorId = new SelectList(_repSetor.GetAll(), "SetorId", "Nome");
            ViewBag.EmpresaId = new SelectList(_repEmp.GetAll(), "EmpresaId", "RazaoSocial");
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateEmp(Empresa empresa)
        {
            var msg = "";
            //var EmpresaDomain = Mapper.Map<EmpresaViewModel, Empresa>(empresa);
            if (ModelState.IsValid)
            {
                _rep.Inserir(empresa);
                msg = "Concluido, inserido com sucesso!";
            }
            else
            {
                msg = "Ocorreu um erro verifique os campos do formulário";
            }
            return Json(new { Message = msg, EmpresaId = empresa.EmpresaId });

        }
        // GET: Empresas/Edit/5
        public ActionResult Edit(int id)
        {
            ContextRDV db=  new ContextRDV();
            Empresa empresa = _rep.RecuperarPorID(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }

            var stemp = db.SetorEmp.Where(se => se.EmpresaId == id).Select(se => se.SetorId);
            ViewBag.SetorId = new SelectList(db.Setor.Where(p=> !stemp.Contains(p.SetorId)), "SetorId", "Nome");
            ViewBag.SetorEmp = new SelectList(db.SetorEmp.Where(se=>se.EmpresaId == id), "SetorId", "SetorDesc");
            var EmpresaModel = Mapper.Map<Empresa,EmpresaViewModel>(empresa);
            return View(EmpresaModel);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Empresa empresa)
        {
            ContextRDV db = new ContextRDV();
            var msg = "";
            if (ModelState.IsValid)
            {
                //var EmpresaDomain = Mapper.Map< EmpresaViewModel,Empresa>(empresa);
                _rep.Alterar(empresa);
                msg = "Concluido, Empresa Atualizada!";
            }
            else
            {
                msg = "Ocorreu um erro verifique os campos do formulário";
            }
            return Json(new { Message = msg});
        }
        // GET: Empresas/Delete/5
        public ActionResult Delete(int id)
        {
            Empresa empresa = _rep.RecuperarPorID(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            var EmpresaDomain = Mapper.Map<Empresa, EmpresaViewModel>(empresa);
            return View(EmpresaDomain);
        }
        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empresa empresa = _rep.RecuperarPorID(id);
            _rep.Remover(empresa);
            return RedirectToAction("Index");
        }
        //GRID
        public JsonResult GetEmpresa(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var EmpList = _rep.GetAll().Select(
                t => new
                {
                    t.EmpresaId,
                    t.RazaoSocial,
                    t.Complemento,
                    t.CodSiga,
                    t.Ativo
                });
            int totalRecords = EmpList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                EmpList = EmpList.OrderByDescending(t => t.EmpresaId);
                EmpList = EmpList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                EmpList = EmpList.OrderBy(t => t.EmpresaId);
                EmpList = EmpList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = EmpList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _rep.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
