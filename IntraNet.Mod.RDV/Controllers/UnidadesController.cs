using System.Linq;
using System.Web.Mvc;
using IntraNet.Data.Repositories;
using IntraNet.Domain.Entities;
using System;
using IntraNet.Mod.RDV.Models.ViewModel;
using AutoMapper;
using IntraNet.Data.Context;

namespace IntraNet.Mod.RDV.Controllers
{
    public class UnidadesController : Controller
    {
        private UnidadeRepository _rep = new UnidadeRepository();

        // GET: Unidades
        public ActionResult Index()
        {
            return View(_rep.GetAll().ToList());
        }

        // GET: Unidades/Details/5
        public ActionResult Details(int id)
        {
            Unidade unidade = _rep.RecuperarPorID(id);
            if (unidade == null)
            {
                return HttpNotFound();
            }
            return View(unidade);
        }

        // GET: Unidades/Create
        public ActionResult Create()
        {
            ContextRDV db = new ContextRDV();
            ViewBag.EmpresaId = new SelectList(db.Empresa, "EmpresaId", "RazaoSocial");
            return View();
        }

        // POST: Unidades/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UnidadeId,Nome,EmpresaId,Observacao")] UnidadeViewModel unidade)
        {
            var msg = "";
            if (ModelState.IsValid)
            {
                var UnidadeDomain = Mapper.Map<UnidadeViewModel, Unidade>(unidade);
                _rep.Inserir(UnidadeDomain);
                msg = "Inserido com sucesso!";
                //return Json(new { Message = msg });
            }
            else
            {
                msg = "Ocorreu um erro, verifique os campos!";
            }
            ViewBag.Mensagem = msg;
            return RedirectToAction("Index", "Administrador");
        }
        public JsonResult GetUnidade(string sidx, string sort, int page, int rows)
        {
            ContextRDV db =new ContextRDV();
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var UnidList = (from t in db.Unidade.AsEnumerable()
                            select new
                            {
                                t.UnidadeId,
                                t.Nome,
                                t.UnidadeClasse,
                                t.Cnpj,
                                t.Edereco,
                                t.Estado
                            });

            
            int totalRecords = UnidList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                UnidList = UnidList.OrderByDescending(t => t.UnidadeId);
                UnidList = UnidList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                UnidList = UnidList.OrderBy(t => t.UnidadeId);
                UnidList = UnidList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = UnidList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        // GET: Unidades/Edit/5
        public ActionResult Edit(int id)
        {
            Unidade unidade = _rep.RecuperarPorID(id);
            var UnidadeViewModel = Mapper.Map<Unidade,UnidadeViewModel>(unidade);
            if (unidade == null)
            {
                return HttpNotFound();
            }
            return View(UnidadeViewModel);
        }

        // POST: Unidades/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UnidadeId,Nome")] UnidadeViewModel unidade)
        {
            var UnidadeDomain = Mapper.Map<UnidadeViewModel, Unidade>(unidade);
            if (ModelState.IsValid)
            {
                _rep.Alterar(UnidadeDomain);
                return RedirectToAction("Index");
            }
            return View(unidade);
        }

        // GET: Unidades/Delete/5
        public ActionResult Delete(int id)
        {
            Unidade unidade = _rep.RecuperarPorID(id);
            var UnidadeViewModel = Mapper.Map<Unidade, UnidadeViewModel>(unidade);
            if (unidade == null)
            {
                return HttpNotFound();
            }
            return View(UnidadeViewModel);
        }

        // POST: Unidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Unidade unidade = _rep.RecuperarPorID(id);
            _rep.Remover(unidade);
            return RedirectToAction("Index");
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
