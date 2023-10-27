using AutoMapper;
using IntraNet.Data.Context;
using IntraNet.Mod.SGR.Models.ViewModel;
using IntraNet.Security.ContextIdentity;
using IntraNet.Security.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IntraNet.Mod.SGR.Controllers
{
    public class SetorController : Controller
    {
        public ApplicationDbContext db_ = new ApplicationDbContext();
        public ContextSGR db = new ContextSGR();
        // GET: Setor
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SetorId,Nome,Observacao")] SetorViewModel setor)
        {
            var msg = "";
            if (ModelState.IsValid)
            {
                var SetorDomain = Mapper.Map<SetorViewModel, Setor>(setor);
                db_.Setor.Add(SetorDomain);
                db_.SaveChanges();
                msg = "Concluido, inserido com sucesso!";
                this.ViewBag.Mensagem = msg;
                return RedirectToAction("Index", "Administrador");
            }
            else
            {
                msg = "Ocorreu um erro verifique os campos do formulário";
                this.ViewBag.Mensagem = msg;
                //return Content("<script language='javascript' type='text/javascript'>alert('Não é possivel inserir o setor!')</ script>");
            }
            return RedirectToAction("Index", "Administrador");
        }

        // GET: Setores/Edit/5
        public ActionResult Edit(int id)
        {
            Setor setor = db_.Setor.Where(set=>set.SetorId==id).AsEnumerable().SingleOrDefault();

            if (setor == null)
            {
                return HttpNotFound();
            }
            return View(setor);
        }

        // POST: Setores/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SetorId,Nome,Observacao")] Setor setor)
        {
            if (ModelState.IsValid)
            {
                db_.Setor.AddOrUpdate(setor);
                db_.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(setor);
        }

        // GET: Setores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setor setor = db_.Setor.Find(id);
            if (setor == null)
            {
                return HttpNotFound();
            }
            return View(setor);
        }
        public JsonResult GetSetor(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var EmpList = db_.Setor.ToList().Select(
                t => new
                {
                    t.SetorId,
                    t.Nome,
                    t.Observacao
                });
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

        // POST: Setores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Setor setor = db_.Setor.Find(id);
            db_.Setor.Remove(setor);
            db_.SaveChanges();
            return RedirectToAction("Index");
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