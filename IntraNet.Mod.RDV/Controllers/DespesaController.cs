using System;
using System.Web.Mvc;
using IntraNet.Domain.Entities;
using System.IO;
using System.Linq;
using AutoMapper;
using IntraNet.Data.Repositories;
using IntraNet.Mod.RDV.Models.ViewModel;
using IntraNet.Data.Context;
using System.Collections.Generic;
using System.Data.Entity.Validation;

namespace IntraNet.Mod.RDV.Controllers
{
    public class DespesaController : Controller
    {
        DespesaRepository _rep = new DespesaRepository();
        RelatorioRepository _repRelat = new RelatorioRepository();
        BaseRepository<DespesaAnexo> _Reposit = new BaseRepository<DespesaAnexo>();
        ContextRDV db = new ContextRDV();
        // GET: Despesa
        public ActionResult Index(int id)
        {
            //Response.AddHeader("Refresh", "5");
            var despesas = _rep.GetAll().Where(desp=>desp.RelatorioId == id);
            return View(despesas.ToList());
        }
        public ActionResult TotalDespesas(int id)
        {            
            decimal TotalDespesas = 00;
            ViewBag.Total = TotalDespesas;
            if (db.Despesas.Where(d => d.RelatorioId == id).Count() <=0)
            {
                ViewBag.Total = "00";
            }else
            {
                ViewBag.Total = db.Despesas.Where(d => d.RelatorioId == id).Select(d => d.Valor).Sum();
            }             
            return View();
        }
        // GET: Despesa/Details/5
        public ActionResult Details(int id)
        {
            Despesas despesas = _rep.RecuperarPorID(id);
            if (despesas == null)
            {
                return HttpNotFound();
            }
            return View(despesas);
        }

        // GET: Despesa/Create
        public ActionResult Create(int id)
        {
            ViewBag.TipoDespesaId = new SelectList(db.TipoDespesa, "TipoDespesaId", "Tipo");
            ViewBag.RelatorioId = _repRelat.GetAll().Where(r => r.RelatorioId == id).Select(r => r.RelatorioId).SingleOrDefault().ToString();
            ViewBag.UserId = _repRelat.GetAll().Where(u => u.RelatorioId == id).Select(u => u.UserId).SingleOrDefault().ToString();
            //ViewBag.RelatorioId = new SelectList(db.Relatorio, "RelatorioId", "RelatorioId");
            return View();
        }

        [HttpPost]
        public JsonResult CreateDesp(DespesasViewModel despesas)
        {
            string msg = "";
            var despcount = db.Despesas.Where(d => d.RelatorioId == despesas.RelatorioId).Count();
            var despDomain = Mapper.Map<DespesasViewModel, Despesas>(despesas);

            if (ModelState.IsValid && despesas.TipoDespesaId != 11 && despesas.Valor > 0 && despcount < 19)
            {
                db.Despesas.Add(despDomain);
                db.SaveChanges();
                DespesasAnexoController controlanexo = new DespesasAnexoController();
                //return Content(@"<script language='javascript' type='text/javascript'>alert('Concluido, desepesa inserida!');window.location.replace('/Relatorio/Edit/" + string.Concat(despesas.RelatorioId) + "');</script>");
                msg = "Sua despesa foi computada com sucesso!";
                return Json(new { success = true, message = msg, isRedirect = true }, JsonRequestBehavior.AllowGet);
            }
            else if (despcount >= 19)
            {
                msg = "Você atingiu numero maximo de despesas por relatório!";
                return Json(new { success = "false", message = msg, isRedirect = true }, JsonRequestBehavior.AllowGet);
            }
            else if (despesas.TipoDespesaId == 11 && despesas.Valor > 0)
            {
                _rep.Inserir(despDomain);
                msg = "Despesa de KM foi computada com sucesso!";
                return Json(new { success = true, message = msg, isRedirect = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var errors = new List<string>();
                foreach (var modelStateVal in ViewData.ModelState.Values)
                {
                    errors.AddRange(modelStateVal.Errors.Select(error => error.ErrorMessage + "</br>"));
                }
                return Json(new { success = "false", message = errors }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DespesasId,Descricao,Observacao,Valor,RelatorioId,UserId,ImagePath")] Despesas despesas)
        {
            if (ModelState.IsValid)
            {
               _rep.Alterar(despesas);
                return RedirectToAction("Edit", "Relatorio", new { Id = despesas.RelatorioId });
            }
            ViewBag.RelatorioId = new SelectList(_rep.GetAll(), "RelatorioId", "UserId", despesas.RelatorioId);
            return RedirectToAction("Edit", "Relatorio", new { Id = despesas.RelatorioId });
        }
        public JsonResult GetDescricaoSelect(int id)
        {
            ContextRDV db = new ContextRDV();            
            bool Outros = false;
            if (id == 10 || id == 9)
            {
                Outros = true;
                return Json(new { Outros = Outros });
            }else if(id == 11)
            {
                Outros = false;
                return Json(new { Outros = Outros, Veiculo = true });
            }                       
            return Json(new { Outros = false });
        }
        public JsonResult GetVeiculoSelect(int id)
        {
            ContextRDV db = new ContextRDV();
            var valor = id;
            var km = db.KMValor;
            foreach(var i in km)
            {
                if (valor >= i.De && valor <= i.Ate)
                {
                    var valorfinal = valor * i.Valor;
                    return Json(new { Valor = valorfinal });
                }
            }
            return Json(new { Valor = 0 });
        }
        // GET: Despesa/Delete/5
        public ActionResult Delete(int id)
        {
            Despesas despesas = _rep.RecuperarPorID(id);
            if (despesas == null)
            {
                return HttpNotFound();
            }
            return View(despesas);
        }

        // POST: Despesa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Despesas despesas = _rep.RecuperarPorID(id);
            _rep.Remover(despesas);
            return RedirectToAction("Edit", "Relatorio", new { Id = despesas.RelatorioId });
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
