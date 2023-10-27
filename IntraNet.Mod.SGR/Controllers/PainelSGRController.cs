using IntraNet.Data.Context;
using IntraNet.Security.ContextIdentity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace IntraNet.Mod.SGR.Controllers
{
    [Authorize]
    public class PainelSGRController : Controller
    {
        public ApplicationDbContext db_ = new ApplicationDbContext();
        public ContextSGR db= new ContextSGR();
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult Grafico()
        {
            var demandasA = db.userdemanda.Where(u =>
                u.Delete != true && u.demanda.Situacao == "Aberto" && u.UserId == User.Identity.Name ||
                u.demanda.Situacao == "Aberto" && u.demanda.Quem == User.Identity.Name).Count();
            var bytes = new Chart(width: 700, height: 300)
                .AddTitle("2018")
                .AddLegend("Resumo de demandas")
                .AddSeries(
                    name: "Total", yFields: "teste",
                    xValue: new[] { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Desembro" },
                    yValues: new[] { "1", "2","0","5","20", "", "", "", "", "", "", "" })
                .GetBytes("png");
            return File(bytes, "image/png");
        }

        [Authorize]
        public ActionResult Dash()
        {
            ViewBag.DemandasAbertas = 0;
            ViewBag.DemandasEncerrado = 0;
            ViewBag.DemandasSuspenso = 0;
            ViewBag.itens = 0;
            ViewBag.Assuntos = 0;
            ViewBag.DemandasTotais = 0;
            var reunao_lista = db.userreuniao.Where(u => u.Delete != false && u.UserName == User.Identity.Name).Select(u=>u.ReuniaoId)
                .ToList();
            if (db.Assunto.Where(a => a.Delete != true && reunao_lista.Contains(a.ReuniaoId)).Count() > 0)
            {
                ViewBag.Assuntos = db.Assunto.Where(a => a.Delete != true && reunao_lista.Contains(a.ReuniaoId)).Count();
                if (db.itemassunto.Where(i => i.Delete != true && reunao_lista.Contains(i.ReuniaoId)).Count() > 0)
                {
                    ViewBag.Itens = db.itemassunto.Where(i => i.Delete != true && reunao_lista.Contains(i.ReuniaoId)).Count();
                    if (db.Demanda.Where(d => d.Delete != true && reunao_lista.Contains(d.ReuniaoId)).Count() > 0)
                    {
                        ViewBag.DemandasTotais = db.Demanda.Where(d => d.Delete != true && reunao_lista.Contains(d.ReuniaoId)).Count();
                        if (db.Demanda.Where(d => d.Delete != true && d.Situacao == "Aberto" && reunao_lista.Contains(d.ReuniaoId)).Count() > 0)
                        {
                            ViewBag.DemandasAbertas = db.Demanda.Where(d => d.Delete != true && d.Situacao == "Aberto" && reunao_lista.Contains(d.ReuniaoId)).Count();
                        }
                        if (db.Demanda.Where(d => d.Delete != true && d.Situacao == "Encerrado" && reunao_lista.Contains(d.ReuniaoId)).Count() > 0)
                        {
                            ViewBag.DemandasEncerrado = db.Demanda.Where(d => d.Delete != true && d.Situacao == "Encerrado" && reunao_lista.Contains(d.ReuniaoId)).Count();
                        }
                        if (db.Demanda.Where(d => d.Delete != true && d.Situacao == "Suspenso" && reunao_lista.Contains(d.ReuniaoId)).Count() > 0)
                        {
                            ViewBag.DemandasSuspenso = db.Demanda.Where(d => d.Delete != true && d.Situacao == "Suspenso" && reunao_lista.Contains(d.ReuniaoId)).Count();
                        }
                    }
                }
            }

            return View();
        }
        public ActionResult UserAdminSGR()
        {

            return View();
        }
        public ActionResult EmpresaAdminSGR()
        {

            return View();
        }
        public ActionResult UnidadeAdminSGR()
        {

            return View();
        }
        
        public JsonResult GetEmpresa(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var EmpList = db_.Empresa.ToList().Select(
                t => new
                {
                    t.EmpresaId,
                    t.RazaoSocial,
                    t.Complemento,
                    t.CodSiga,
                    t.Ativo
                });
            int totalRecords = EmpList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                EmpList = EmpList.OrderByDescending(t => t.EmpresaId);
                EmpList = EmpList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                EmpList = EmpList.OrderBy(t => t.EmpresaId);
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
        public JsonResult GetUnidade(string sidx, string sort, int page, int rows)
        {
            ContextRDV db = new ContextRDV();
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var UnidList = (from t in db.Unidade.AsEnumerable()
                join emp in db.Empresa on t.EmpresaId equals emp.EmpresaId 
                select new
                {
                    t.UnidadeId,
                    t.Nome,
                    t.UnidadeClasse,
                    Empresa = emp.RazaoSocial,
                    t.EmpresaId,
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


        public ActionResult Config()
        {
            return View();
        }

    }
}