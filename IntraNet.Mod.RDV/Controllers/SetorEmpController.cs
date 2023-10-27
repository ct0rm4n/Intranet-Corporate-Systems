using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities;
using IntraNet.Data.Repositories;

namespace IntraNet.Mod.RDV.Controllers
{
    public class SetorEmpController : Controller
    {
        private SetorEmpRepository _rep = new SetorEmpRepository();
        EmpresaRepository _repEmp = new EmpresaRepository();
        SetorRepository _repDepto = new SetorRepository();
        // GET: SetorEmp
        public ActionResult Index()
        {
            var empresaS =_rep.GetAll().Include(e => e.setor).Include(e => e.empresa);
            return View(empresaS.ToList());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateSetorEmp(int[] listarray, int EmpresaId)
        {
            ContextRDV db = new ContextRDV();
            
            var msg = "";
            if (ModelState.IsValid)
            {
                var remove = db.SetorEmp.Where(se => se.EmpresaId == EmpresaId).ToList();
                if (remove.Count > 0)
                {
                    //quando existir departamentos na empresa
                    foreach (var item in remove)
                    {
                        db.SetorEmp.Remove(item);
                        db.SaveChanges();
                    }
                }
                if (listarray.Length > 0)
                {
                    foreach (var item in listarray)
                    {
                        //percorre o vetor
                        var setornome = db.Setor.Where(s => s.SetorId == item).Select(s => s.Nome).SingleOrDefault();
                        System.Diagnostics.Debug.WriteLine("Setor a ser associado a empresa é:" + item);
                        var items = new SetorEmp()
                        {
                            //cria o objeto item com os dados, para inserir na tabela itemreuniao
                            SetorDesc = setornome,
                            EmpresaId = EmpresaId,
                            SetorId = item
                        };
                        _rep.Inserir(items);
                        msg = "Lista de Setors associado com sucesso";
                        System.Diagnostics.Debug.WriteLine("Salvo:");
                    }
                }
            }
            else
            {
                msg = "Ocorreu um erro verifique os campos do formulário";
            }
            return Json(new { Message = msg });

        }

        // GET: SetorEmp/Details/5
        public ActionResult Details(int id)
        {
            SetorEmp setorEmp = _rep.RecuperarPorID(id);
            if (setorEmp == null)
            {
                return HttpNotFound();
            }
            return View(setorEmp);
        }

        // GET: SetorEmp/Create
        public ActionResult Create()
        {
            
            ViewBag.SetorId = new SelectList(_repDepto.GetAll(), "SetorId", "Nome");
            ViewBag.EmpresaId = new SelectList(_repEmp.GetAll(), "EmpresaId", "RazaoSocial");
            return View();
        }

        // POST: SetorEmp/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SetorEmpId,SetorId,EmpresaId")] SetorEmp setorEmp)
        {
            
            if (ModelState.IsValid)
            {
                _rep.Inserir(setorEmp);
                return RedirectToAction("Index");
            }

            ViewBag.SetorId = new SelectList(_repDepto.GetAll(), "SetorId", "Nome", setorEmp.SetorId);
            ViewBag.EmpresaId = new SelectList(_repDepto.GetAll(), "EmpresaId", "RazaoSocial", setorEmp.EmpresaId);
            return View(setorEmp);
        }

        // GET: SetorEmp/Edit/5
        public ActionResult Edit(int id)
        {

            SetorEmp setorEmp = _rep.RecuperarPorID(id);
            if (setorEmp == null)
            {
                return HttpNotFound();
            }
            ViewBag.SetorId = new SelectList(_repDepto.GetAll(), "SetorId", "Nome", setorEmp.SetorId);
            ViewBag.EmpresaId = new SelectList(_repEmp.GetAll(), "EmpresaId", "RazaoSocial", setorEmp.EmpresaId);
            return View(setorEmp);
        }

        // POST: SetorEmp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SetorEmpId,SetorId,EmpresaId")] SetorEmp setorEmp)
        {
            if (ModelState.IsValid)
            {
                _rep.Alterar(setorEmp);
                return RedirectToAction("Index");
            }
            return View(setorEmp);
        }

        // GET: SetorEmp/Delete/5
        public ActionResult Delete(int id)
        {

            SetorEmp setorEmp = _rep.RecuperarPorID(id);
            if (setorEmp == null)
            {
                return HttpNotFound();
            }
            return View(setorEmp);
        }

        // POST: SetorEmp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var empdp =_rep.RecuperarPorID(id);
            _rep.Remover(empdp);
            return RedirectToAction("Index");
        }
        public JsonResult GetSetorEmpSubgrid(int ID,string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var EmpList = _rep.GetAll().Select(
                t => new
                {
                    t.SetorEmpId,
                    t.SetorId,
                    t.SetorDesc,
                    t.EmpresaId
                }).Where(t=>t.EmpresaId ==ID);

            int totalRecords = EmpList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                EmpList = EmpList.OrderByDescending(t => t.SetorId);
                EmpList = EmpList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                EmpList = EmpList.OrderBy(t => t.SetorId);
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

