using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using IntraNet.Data.Context;
using IntraNet.Data.Repositories;
using IntraNet.Domain.Entities;
using System;
using IntraNet.Mod.RDV.Models.ViewModel;
using AutoMapper;

namespace IntraNet.Mod.RDV.Controllers
{
    [Authorize]
    public class SetorController : Controller
    {
        private SetorRepository _rep = new SetorRepository();
        private ContextRDV db = new ContextRDV();

        // GET: Setores
        public ActionResult Index()
        {
            return View(db.Setor.ToList());
        }

        // GET: Setores/Details/5
        public ActionResult Details(int id)
        {
            Setor setor = _rep.RecuperarPorID(id);
            if (setor == null)
            {
                return HttpNotFound();
            }
            return View(setor);
        }

        // GET: Setores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Setores/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SetorId,Nome,Observacao")] SetorViewModel setor)
        {
            var msg = "";
            if (ModelState.IsValid)
            {
                var SetorDomain = Mapper.Map<SetorViewModel, Setor>(setor);
                _rep.Inserir(SetorDomain);
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
            Setor setor = _rep.RecuperarPorID(id);
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
                _rep.Alterar(setor);
                db.SaveChanges();
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
            Setor setor = db.Setor.Find(id);
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

            var EmpList = _rep.GetAll().Select(
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
            Setor setor = db.Setor.Find(id);
            db.Setor.Remove(setor);
            db.SaveChanges();
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
