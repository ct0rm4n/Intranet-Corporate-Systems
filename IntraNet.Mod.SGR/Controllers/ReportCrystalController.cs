using CrystalDecisions.CrystalReports.Engine;
using IntraNet.Data.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntraNet.Mod.SGR.Controllers
{
    public class ReportCrystalController : Controller
    {
        // GET: ReportCrystal
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult UserReportAta(int id)
        {
            ContextSGR db = new ContextSGR();
            ReportDocument rdSGR = new ReportDocument();
            rdSGR.Load(Path.Combine(Server.MapPath(@"~/Reports/"), "RelatorioAta.rpt"));
            ReportDocument report = new ReportDocument();

            var relatorio =  (from item in db.itemassunto.Distinct()
                              select new
                              {   
                                  item.Assunto.DescricaoAs,
                                  item.ItemAssuntoId,
                                  item.AssuntoId,
                                  item.ReuniaoId,
                                  item.Situacao,
                                  item.DescricaoItem,
                                  item.Responsavel,
                                  item.InseridoEm,
                                  item.QuemInseriu,
                                  item.Delete,
                                  AssuntoDel = item.Assunto.Delete,
                              }).Where(item => item.ReuniaoId == id && item.Delete == false && item.AssuntoDel == false).ToList();

            rdSGR.SetDatabaseLogon("sa", "sa", "localhost", "data_SGR");
            rdSGR.SetDataSource(relatorio);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {                
                //GeraPDF
                Stream stream = rdSGR.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "RelatorioReuniao_"+id+".pdf");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("erro: " + e);
                throw;
            }
        }
        public ActionResult UserReportDemandaPorReuniao(int id)
        {
            ContextSGR db = new ContextSGR();
            ReportDocument rdSGR = new ReportDocument();
            rdSGR.Load(Path.Combine(Server.MapPath(@"~/Reports/"), "DemandasAcoes_Reuniao.rpt"));
            //ReportDocument report = new ReportDocument();

            var relatorio = (from d in db.Demanda.Where(d => d.ReuniaoId == id && d.Delete == false).AsEnumerable()
                             join r in db.reuniao on d.ReuniaoId equals r.ReuniaoId
                             join t in db.itemassunto on d.ItemAssuntoId equals t.ItemAssuntoId
                             join a in db.Assunto on t.AssuntoId equals a.AssuntoId
                             select new
                             {
                                 d.DemandaId,
                                 r.Nome,
                                 t.DescricaoItem,
                                 a.DescricaoAs,
                                 a.AssuntoId,
                                 d.Situacao,
                                 d.ReuniaoId,
                                 d.ItemAssuntoId,
                                 d.Oque,
                                 d.Como,
                                 d.Porque,
                                 d.Quanto,
                                 d.Quem,
                                 d.Onde,
                                 d.Quando,
                             }).Distinct().ToList();

            rdSGR.SetDatabaseLogon("sa", "sa", "localhost", "data_SGR");
            rdSGR.SetDataSource(relatorio);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                //GeraPDF
                Stream stream = rdSGR.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "RelatorioReuniao_" + id + ".pdf");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("erro: " + e);
                throw;
            }
        }
        public ActionResult UserReportDemandaPorItem(int id)
        {
            ContextSGR db = new ContextSGR();
            ReportDocument rdSGR = new ReportDocument();
            rdSGR.Load(Path.Combine(Server.MapPath(@"~/Reports/"), "DemandasAcoes.rpt"));
            //ReportDocument report = new ReportDocument();

            var relatorio = (from d in db.Demanda.Where(d => d.ItemAssuntoId == id && d.Delete == false).AsEnumerable()
                             join r in db.reuniao on d.ReuniaoId equals r.ReuniaoId
                             join t in db.itemassunto on d.ItemAssuntoId equals t.ItemAssuntoId                
                            join a in db.Assunto on t.AssuntoId equals a.AssuntoId
                            join acao in db.AcaoDemanda.Distinct().DefaultIfEmpty() on d.DemandaId equals acao.DemandaId                                          
                            select new
                            {
                                d.DemandaId,
                                r.Nome,
                                t.DescricaoItem,
                                a.DescricaoAs,
                                a.AssuntoId,                                
                                d.Situacao,
                                d.ReuniaoId,
                                d.ItemAssuntoId,
                                d.Oque,
                                d.Como,
                                d.Porque,
                                d.Quanto,
                                d.Quem,
                                d.Onde,
                                d.Quando,
                             }).Distinct().ToList();

            rdSGR.SetDatabaseLogon("sa", "sa", "localhost", "data_SGR");
            rdSGR.SetDataSource(relatorio);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                //GeraPDF
                Stream stream = rdSGR.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "RelatorioReuniao_" + id + ".pdf");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("erro: " + e);
                throw;
            }
        }
    }
}