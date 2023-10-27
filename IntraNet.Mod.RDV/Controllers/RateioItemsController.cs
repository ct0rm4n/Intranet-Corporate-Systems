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
    public class RateioItemsController : Controller
    {
        private ContextRDV db = new ContextRDV();

        // GET: RateioItems
        public ActionResult Index(int id)
        {
            var rateioItems = db.RateioItems.Where(r => r.RelatorioId==id).Include(r => r.relatorio).Include(r => r.empsCCusto)
                .Include(u => u.unidade);
            return View(rateioItems.ToList());
        }

        // GET: RateioItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateioItem rateioItem = db.RateioItems.Find(id);
            if (rateioItem == null)
            {
                return HttpNotFound();
            }
            return View(rateioItem);
        }

        // GET: RateioItems/Create
        public ActionResult Create(int id,string UserId)
        {
            var empid = db.Relatorio.Where(e => e.RelatorioId == id).Select(e => e.EmpresaId).FirstOrDefault();
            var querysolicita = from se in db.SolicitaCusto.Where(se => se.UserId == UserId && se.EmpresaId == empid)
                join cc in db.EmpresaCC on se.EmpCCustoId equals cc.EmpCCustoId
                select new
                {
                    se.SolicitaCCustoId,
                    cc.CCustoDesc
                };
            ViewBag.Item = db.SolicitaEmpresas.Where(s => s.UserId == UserId && s.EmpresaId == empid)
                .Select(s => s.Fornecedor).SingleOrDefault();
            ViewBag.SolicitaCCustoId = new SelectList(querysolicita, "SolicitaCCustoId", "CCustoDesc");
            ViewBag.EmpCCustoId = new SelectList(db.EmpresaCC, "EmpCCustoId", "CCusto");
            ViewBag.UnidadeClasse = new SelectList(db.Unidade, "UnidadeId", "Nome");
            
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "RateioItemId")] RateioItem rateioItem)
        {
            var msg = "";
            var inserido = false;
            //return RedirectToAction("Index", "Relatorio",new {Id= rateioItem.RelatorioId });
            var cont= db.RateioItems.Where(d => d.RelatorioId == rateioItem.RelatorioId).Count();
            var desttotal = 0;
            if (cont > 0)
            {
                desttotal = db.RateioItems.Where(d => d.RelatorioId == rateioItem.RelatorioId).Select(d => d.Porcentagem).Sum();

            }
            if (desttotal+rateioItem.Porcentagem <= 100 & desttotal+ rateioItem.Porcentagem >= 0)
            {
                if (ModelState.IsValid)
                {
                    
                    
                    db.RateioItems.Add(rateioItem);
                    db.SaveChanges();
                    msg = "Concluido";
                    //return Json(new { Message = msg, Id = rateioItem.RelatorioId });
                    inserido = true;
                }
                else
                {
                    msg = "Ocorreu um erro verifique o formulario";
                }
            }
            else
            {
                msg = "Não foi possivel inserir, a porcentagem do item excede o limite do rateio";
            }
            return Json(new { Message = msg, Id = rateioItem.RelatorioId, inserido=inserido });
        }

        // GET: RateioItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateioItem rateioItem = db.RateioItems.Find(id);
            if (rateioItem == null)
            {
                return HttpNotFound();
            }
            return View(rateioItem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RateioItemId,SolicitanteCC,EmpCC,Classe,RelatorioId,SolicitaEmpresaId")] RateioItem rateioItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rateioItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rateioItem);
        }

        // GET: RateioItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateioItem rateioItem = db.RateioItems.Find(id);
            if (rateioItem == null)
            {
                return HttpNotFound();
            }
            return View(rateioItem);
        }

        // POST: RateioItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RateioItem rateioItem = db.RateioItems.Find(id);
            db.RateioItems.Remove(rateioItem);
            db.SaveChanges();
            return RedirectToAction("Edit","Relatorio", new { id=rateioItem.RelatorioId});
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
