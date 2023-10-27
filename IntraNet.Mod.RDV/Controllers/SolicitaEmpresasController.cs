using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities;

namespace IntraNet.Mod.RDV.Controllers
{
    public class SolicitaEmpresasController : Controller
    {
        private ContextRDV db = new ContextRDV();

        public ActionResult Index(string id)
        {
            var solicitaEmpresas = db.SolicitaEmpresas.Include(s => s.empresa).Where(s=>s.UserId == id);
            return View(solicitaEmpresas.ToList());
        }

        public ActionResult Create(string id)
        {
            //var solictemp = db.SolicitaEmpresas.Where(se => se.UserId == id).Select(se => se.EmpresaId);
            ViewBag.UserId = id;
            ViewBag.EmpresaId = new SelectList(db.Empresa.Where(emp=> emp.EmpresaId!=99), "EmpresaId", "RazaoSocial");
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SolicitaEmpresaId,EmpresaId,UserId,Fornecedor,Projeto")] SolicitaEmpresa solicitaEmpresa)
        {
            var msg = "";
            bool inserido = false;
            if (ModelState.IsValid)
            {
                try
                {
                    inserido = true;
                    db.SolicitaEmpresas.Add(solicitaEmpresa);
                    db.SaveChanges();
                    msg = "Inserido";
                    return Json(new { Message = msg, Status = inserido });
                }
                catch (Exception e)
                {
                    msg = "Ocorreu um erro, verifique o cabeçalho antes de posseguir";
                    inserido = false;
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                inserido = false;
                msg = "Ocorreu um erro, verifique o cabeçalho antes de posseguir";
            }
            return Json(new { Message = msg, Status = inserido });
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SolicitaEmpresa solicitaEmpresa = db.SolicitaEmpresas.Find(id);
            if (solicitaEmpresa == null)
            {
                return HttpNotFound();
            }
            return View(solicitaEmpresa);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SolicitaEmpresa solicitaEmpresa = db.SolicitaEmpresas.Find(id);
            IList<SolicitaCCusto> remove = db.SolicitaCusto.Where(i => i.SolicitaEmpresaId == id).ToList();
            if (remove.Count > 0)
            {
                //quando existir departamentos na empresa
                foreach (var item in remove)
                {
                    db.SolicitaCusto.Remove(item);
                    db.SaveChanges();
                }
            }
            db.SolicitaEmpresas.Remove(solicitaEmpresa);
            db.SaveChanges();
            return RedirectToAction("Edit", "UsersAdmin", new {id=solicitaEmpresa.UserId});
        }

        [HttpPost]
        public ActionResult GetCCSelect(int id)
        {
            ContextRDV db = new ContextRDV();
            var msg = "";
            var CCList = new SelectList(db.EmpresaCC, "EmpCCustoId", "CCustoDescs");
            var proj = db.SolicitaEmpresas.Where(i => i.SolicitaEmpresaId == id).Select(i => i.Projeto)
                .SingleOrDefault();
            var solicitaemp = db.SolicitaEmpresas.Where(s => s.SolicitaEmpresaId == id).Select(s => s.EmpresaId);
            if (id > 0)
            {
                CCList = null;
                if (proj == true)
                {
                    CCList = new SelectList(db.EmpresaCC.Where(und => solicitaemp.Contains(und.EmpresaId) && und.Projeto == true || und.EmpresaId == 99), "EmpCCustoId", "CCustoDesc");
                }
                else
                {
                    CCList = new SelectList(db.EmpresaCC.Where(und => solicitaemp.Contains(und.EmpresaId) && und.Projeto != true || und.EmpresaId == 99), "EmpCCustoId", "CCustoDesc");
                }

            }
            else
            {
                msg = "Verifique o formulário!";
            }
            return Json(new { Message = msg, Ccusto = CCList });
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
