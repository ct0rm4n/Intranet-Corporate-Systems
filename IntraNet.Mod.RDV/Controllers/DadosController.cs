using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities;
using IntraNet.Data.Repositories;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace IntraNet.Mod.RDV.Controllers
{
    [Authorize]
    public class DadosController : Controller
    {
        private ContextRDV db = new ContextRDV();
        private BaseRepository<DadosBancarios> _rep = new BaseRepository<DadosBancarios>();


        // GET: Dados
        public ActionResult Index()
        {
            string iduser = User.Identity.GetUserName();
            var listadados = _rep.GetAll().Where(n => n.UserId == iduser).ToList();
            return View(listadados);

        }

        // GET: Dados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DadosBancarios dadosBancarios = db.DadosBancarios.Find(id);
            if (dadosBancarios == null)
            {
                return HttpNotFound();
            }
            return View(dadosBancarios);
        }

        // GET: Dados/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: Dados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DadosBancariosId,UserId,Agencia,ContaCorrente,Cpf")] DadosBancarios dadosBancarios)
        {
            if (ModelState.IsValid)
            {
                db.DadosBancarios.Add(dadosBancarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dadosBancarios);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateModel([Bind(Include = "DadosBancariosId,UserId,Banco,Agencia,Dv,ContaCorrente,Cpf")] DadosBancarios dadosBancarios)
        {
            var msg = "";
            if (ModelState.IsValid)
            {
                _rep.Inserir(dadosBancarios);
                msg = "Inserido com sucesso!";
            }
            else
            {
                msg = "Verifique o formulário!";
            }
            return Json(new { Message = msg });
        }
        // GET: Dados/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DadosBancarios dadosBancarios = db.DadosBancarios.Find(id);
            if (dadosBancarios == null)
            {
                return HttpNotFound();
            }
            return View(dadosBancarios);
        }

        // POST: Dados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DadosBancariosId,UserId,Agencia,ContaCorrente,Cpf")] DadosBancarios dadosBancarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dadosBancarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dadosBancarios);
        }

        // GET: Dados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DadosBancarios dadosBancarios = db.DadosBancarios.Find(id);
            if (dadosBancarios == null)
            {
                return HttpNotFound();
            }
            return View(dadosBancarios);
        }

        // POST: Dados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DadosBancarios dadosBancarios = db.DadosBancarios.Find(id);
            db.DadosBancarios.Remove(dadosBancarios);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult CreateDados()
        {
            ViewBag.Banco = new SelectList(new[]
            {
                new {Valor = "Santander", Texto = "Santander"},
                new {Valor = "Banco do Brasil", Texto = "Banco do Brasil"},
                new {Valor = "Itau", Texto = "Itau"},
                new {Valor = "Caixa", Texto = "Caixa"},
                new {Valor = "Bradesco", Texto = "Bradesco"}
            }, "Valor", "Texto");
            string iduser = User.Identity.GetUserName();
            ViewBag.UserId = iduser;
            return View();
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
