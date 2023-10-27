using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities;
using System;
using IntraNet.Data.Repositories;
using SelectListItem = System.Web.WebPages.Html.SelectListItem;

namespace IntraNet.Mod.RDV.Controllers
{
    public class EmpCCustoController : Controller
    {
        private EmpCCustoRepository _rep = new EmpCCustoRepository();
        EmpresaRepository _repEmp = new EmpresaRepository();
        SetorEmpRepository _repEmpDp = new SetorEmpRepository();
        UnidadeRepository _repUnid = new UnidadeRepository();
        // GET: EmpCCusto 
        public ActionResult Index()
        {
            
            var empresaCC = _rep.GetAll();
            return View(empresaCC.ToList());
        }

        // GET: EmpCCusto/Details/5
        public ActionResult Details(int id)
        {
            EmpCCusto empCCusto = _rep.RecuperarPorID(id);
            if (empCCusto == null)
            {
                return HttpNotFound();
            }
            return View(empCCusto);
        }

        // GET: EmpCCusto/Create
        public ActionResult Create()
        {
            var emp = _repEmp.GetAll().OrderByDescending(p => p.EmpresaId).FirstOrDefault().EmpresaId;
            ViewBag.EmpresaId = new SelectList(_repEmp.GetAll().Where(n=>n.EmpresaId == emp), "EmpresaId", "RazaoSocial");

            return View();
        }
        public ActionResult CreateEmpCC()
        {
            //ContextRDV db = new ContextRDV();
            ViewBag.EmpresaId = new SelectList(_repEmp.GetAll(), "EmpresaId", "RazaoSocial");
            ViewBag.EmpresaDeptoId = new SelectList(_repEmpDp.GetAll(), "EmpresaDeptoId", "DepartamentoDesc");
            ViewBag.UnidadeClasse = new SelectList(_repUnid.GetAll(),"UnidadeId","Nome");
            
            return View();
        }

        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateModel([Bind(Include = "EmpCCustoId,CCusto, CCustoDesc, EmpresaId, Projeto,Prospect ,  Ativo, Classe, Item, UnidadeId")] EmpCCusto empresacc)
        {
            var msg = "";
            empresacc.Ativo = true;
            if (ModelState.IsValid)
            {
                if (_rep.GetAll().Where(i => i.CCusto == empresacc.CCusto).Count() == 0 && _rep.GetAll().Where(i => i.CCustoDesc == empresacc.CCustoDesc).Count() == 0)
                {
                    _rep.Inserir(empresacc);
                    msg = "Concluido, inserido com sucesso!";
                }
                else
                {
                    msg = "Esse centro de custo ja está cadastrado!";
                }
            }
            else
            {
                msg = "Verifique o formulário!";
            }
            return Json(new { Message = msg });
        }        

        //GRID
        public JsonResult GetEmpCC( string sidx, string sort, int page, int rows)
        {
            ContextRDV db = new ContextRDV();

            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var EmpCCList = (from u in db.EmpresaCC
                from e in db.Empresa.Where(e=>e.EmpresaId==u.EmpresaId).DefaultIfEmpty()
                from und in db.Unidade.Where(und=>und.UnidadeId == u.UnidadeClasse).DefaultIfEmpty()
                select new
                {
                    u.EmpCCustoId,
                    u.EmpresaId,
                    e.RazaoSocial,
                    Classe = u.UnidadeClasse,
                    Item = u.Item ?? "N/A",
                    Nome = und.Nome ?? "N/A",
                    Projeto = u.Projeto ?? false,
                    Prospect = u.Prospect ?? false,
                    u.CCusto,
                    u.CCustoDesc,
                    u.Ativo
                });

            int totalRecords = EmpCCList.Count();

            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                EmpCCList = EmpCCList.OrderByDescending(t => t.EmpresaId);
                EmpCCList = EmpCCList.Skip(pageIndex * pageSize).Take(pageSize);
            }

            else
            {
                EmpCCList = EmpCCList.OrderBy(t => t.EmpresaId);
                EmpCCList = EmpCCList.Skip(pageIndex * pageSize).Take(pageSize);
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = EmpCCList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        // GET: EmpCCusto/Edit/5
        public string Edit(EmpCCusto Model)
        {
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    _rep.Alterar(Model);
                    msg = "Saved Successfully";
                }
                else
                {
                    msg = "Validation data not successfully";
                }
            }
            catch (Exception ex)
            {
                msg = "Error occured:" + ex.Message;
            }
            return msg;
        }
        public string Delete(int ID)
        {
            EmpCCusto empcc = _rep.GetAll().Where(i => i.EmpCCustoId == ID).Single();
            _rep.Remover(empcc);
            return "Deleted successfully";
        }

        public ActionResult GetUnidadeSelect(int id)
        {
            ContextRDV db = new ContextRDV();
            var msg = "";
            var unidadeList = new SelectList(db.Unidade, "UnidadeId", "Nome");
            if (id > 0)
            {
                unidadeList = null;
                unidadeList = new SelectList(db.Unidade, "UnidadeId", "Nome");
                
            }
            else
            {
                msg = "Verifique o formulário!";
            }
            return Json(new { Message = msg, Unidades = unidadeList });
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
