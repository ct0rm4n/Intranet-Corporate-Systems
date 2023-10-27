using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities;

namespace IntraNet.Mod.RDV.Controllers
{
    public class SolicitaCustoController : Controller
    {
        private ContextRDV db = new ContextRDV();

        // GET: SolicitaCusto
        public ActionResult Index(string id)
        {
            var solicitaCCusto = db.SolicitaCusto.Include(s => s.empccusto).Where(s=>s.UserId == id);
            return View(solicitaCCusto.ToList());
        }

        // GET: SolicitaCusto/Create
        public ActionResult Create(string id)
        {
            //var stemp = db.SolicitaEmpresas.Where(se => se.UserId == id).Select(se => se.SetorId);
            var solictemp = db.SolicitaEmpresas.Where(se => se.UserId == id).Select(se => se.EmpresaId);
            var query = from u in db.SolicitaEmpresas.Where(se => se.UserId == id)
                        join i in db.Empresa on u.EmpresaId equals i.EmpresaId
                        select new
                        {
                            u.SolicitaEmpresaId,
                            i.RazaoSocial
                        };
            
            ViewBag.SolicitaEmpresaId = new SelectList(query, "SolicitaEmpresaId", "RazaoSocial");
            ViewBag.EmpresaId = new SelectList(db.Empresa.Where(emp => !solictemp.Contains(emp.EmpresaId)), "EmpresaId", "RazaoSocial");

            ViewBag.EmpCCustoId = new SelectList(db.EmpresaCC.Where(emp => solictemp.Contains(emp.EmpresaId)), "EmpCCustoId", "CCustoDesc");
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "SolicitaCCustoId")] SolicitaCCusto solicitaCCusto)
        {
            var msg = "";
            var empid = db.SolicitaEmpresas.Where(s => s.SolicitaEmpresaId == solicitaCCusto.SolicitaEmpresaId)
                .Select(s => s.EmpresaId).SingleOrDefault();
            solicitaCCusto.EmpresaId = empid;

            if (ModelState.IsValid)
            {
                db.SolicitaCusto.Add(solicitaCCusto);
                db.SaveChanges();
                msg = "Concluida, inserido com sucesso!";

            }
            else
            {
                msg = "Ocorreu um erro verifique o formulário";
            }
            ViewBag.EmpCCustoId = new SelectList(db.EmpresaCC, "EmpCCustoId", "CCustoDesc", solicitaCCusto.EmpCCustoId);
            return Json(new { Message = msg, Id = solicitaCCusto.UserId });
        }

        // GET: SolicitaCusto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SolicitaCCusto solicitaCCusto = db.SolicitaCusto.Find(id);
            if (solicitaCCusto == null)
            {
                return HttpNotFound();
            }
            return View(solicitaCCusto);
        }

        // POST: SolicitaCusto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SolicitaCCusto solicitaCCusto = db.SolicitaCusto.Find(id);
            db.SolicitaCusto.Remove(solicitaCCusto);
            db.SaveChanges();
            return RedirectToAction("Edit", "UsersAdmin", new { id = solicitaCCusto.UserId });
        }

        [HttpPost]
        public ActionResult GetCCSelect(int ccid)
        {
            ContextRDV db = new ContextRDV();
            var msg = "";
            //var solicita = db.SolicitaCusto.Where(i => i.SolicitaEmpresaId == id).SingleOrDefault();
            if (ccid > 0)
            {
                var emp = db.SolicitaCusto.Where(ep => ep.SolicitaCCustoId == ccid).First();
                var cc = new SelectList(db.EmpresaCC.Where(s => s.EmpCCustoId == emp.EmpCCustoId), "EmpCCustoId",
                    "CCusto");

                var prosp = db.EmpresaCC.Where(i => i.EmpCCustoId == emp.EmpCCustoId).Select(i => i.Projeto).First();
                var classe = new SelectList(db.Unidade, "UnidadeId", "Nome");
                var prospect = false;
                if (prosp == true)
                {
                    prospect = true;
                }

                return Json(new {Message = msg, Ccusto = cc, Classe = classe, Prospect = prospect});
            }
            else
            {
                return null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
