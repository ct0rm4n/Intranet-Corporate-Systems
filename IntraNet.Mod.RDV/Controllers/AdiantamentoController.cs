using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities;
using IntraNet.Mod.RDV.Models.ViewModel;
using IntraNet.Security.ContextIdentity;
using IntraNet.Security.Filters;
using IntraNet.Security.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntraNet.Mod.RDV.Controllers
{   
    [Authorize]
    public class AdiantamentoController : Controller
    {
        ContextRDV db = new ContextRDV();
        ApplicationDbContext db_ = new ApplicationDbContext();
        //ApplicationIdentity db_ = new ApplicationIdentity();
        // GET: Adiantamento
        public ActionResult Index()
        {
            string iduser = User.Identity.GetUserName();
            List<Adiantamento> listadados = db.Adiantamento.Where(n => n.UserId == iduser).OrderByDescending(n => n.AdiantamentoId).ToList();
            var AdiantamentoDomain = Mapper.Map<List<Adiantamento>,List<AdiantamentoViewModel>>(listadados);
            return View(AdiantamentoDomain);
        }
        [ClaimsAuthorize("Financeiro", "VamtecGroup")]
        public ActionResult Financeiro()
        {
            string iduser = User.Identity.GetUserId();
            ApplicationUser user = db_.Users.Find(iduser);
            //List<Adiantamento> listadados = db.Adiantamento.Where(n => n.Situacao == "Aprovado" && n.EmpresaId == user.Local).OrderByDescending(n=>n.AdiantamentoId).ToList();
            //var AdiantamentoDomain = Mapper.Map<List<Adiantamento>, List<AdiantamentoViewModel>>(listadados);
            return View();
        }
        [ClaimsAuthorize("Aprovador", "VamtecGroup")]
        public ActionResult Aprovador()
        {
            string user = User.Identity.GetUserName();
            Aprovador aprov = db.Aprovador.Where(apv=>apv.ArpovName == user).SingleOrDefault();
            List<Adiantamento> listadados = db.Adiantamento.Where(n => n.AprovadorId == aprov.AprovadorId).OrderByDescending(n => n.AdiantamentoId).ToList();
            //ViewBag.DadosBancariosId = new SelectList(db.DadosBancarios.Where(i => i.UserId == nmuser), "DadosBancariosId", "Banco");
            var AdiantamentoDomain = Mapper.Map<List<Adiantamento>, List<AdiantamentoViewModel>>(listadados);
            return View(AdiantamentoDomain);
        }

        public ActionResult Create()
        {
            string iduser = User.Identity.GetUserName();
            ViewBag.iduser = iduser;
            ApplicationUser user = db_.Users.Where(u=>u.UserName ==iduser).SingleOrDefault();
            //var aprovador = db.Aprovador.Where(usr => usr.EmpresaId == user.Local && usr.SetorId == user.Setor).SingleOrDefault();
            ViewBag.DadosBancariosId = new SelectList(db.DadosBancarios.Where(i => i.UserId == iduser), "DadosBancariosId", "Banco");
            //ViewBag.AprovadorId = aprovador.AprovadorId;
            //ViewBag.EmpresaId = user.Local;
            //ViewBag.SolicitaEmpresaId = db.SolicitaEmpresas.Where(s => s.EmpresaId == user.Local && s.UserId == user.Id).Select(s=>s.SolicitaEmpresaId).SingleOrDefault();
            return View();
        }
        //[ClaimsAuthorize("Aprovador", "VamtecGroup")]
        public ActionResult Details(int id)
        {
            Adiantamento adiantamento = db.Adiantamento.Find(id);
            var adiantamentoView = Mapper.Map<Adiantamento,AdiantamentoViewModel>(adiantamento);
            if (adiantamento == null)
            {
                return HttpNotFound();
            }
            return View(adiantamentoView);
        }
        [ClaimsAuthorize("Financeiro", "VamtecGroup")]
        public ActionResult DetailsFinanceiro(int id)
        {
            Adiantamento adiantamento = db.Adiantamento.Find(id);
            var adiantamentoView = Mapper.Map<Adiantamento, AdiantamentoViewModel>(adiantamento);
            if (adiantamento == null)
            {
                return HttpNotFound();
            }
            return View(adiantamentoView);
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> Create(AdiantamentoViewModel adiantaview)
        {
            
            string message = "";
            adiantaview.SolicitadoEm = DateTime.Now;
            if (ModelState.IsValid)
            {
                var AdiantamentoDomain = Mapper.Map<AdiantamentoViewModel, Adiantamento>(adiantaview);
                db.Adiantamento.Add(AdiantamentoDomain);
                db.SaveChanges();
                var link = "http://192.168.0.25:8083/Adiantamento/Aprovador";
                var texto = "<p>O funcionário &nbsp;" + adiantaview.UserId+"&nbsp; está requisitando uma quantia para adiantamento de viagem. Você é o aprovador responsavel do funcionário, abaixo o link para visualizar sua solicitação.</p></br><p><a href='" + link + "'>" + link + "</p>";
                var aprovador = db.Aprovador.Where(ap => ap.AprovadorId == adiantaview.AprovadorId).Select(ap => ap.UserId).SingleOrDefault();
                var emailuser = db_.Users.Where(user=>user.Id==aprovador).Select(user=>user.Email).SingleOrDefault();
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(emailuser, texto, "Solicitação de adiantamento");
                message = "Solicitação encaminhada com sucesso, você recebera um email quando for aprovada.";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var errors = new List<string>();
                foreach (var modelStateVal in ViewData.ModelState.Values)
                {
                    errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                }
                return Json(new { success = false, message = errors });
            }
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> Aprovar(int id)
        {
            Adiantamento adiantamento = db.Adiantamento.Find(id);
            adiantamento.Situacao = "Aprovado";
            db.Entry(adiantamento).State = EntityState.Detached;
            db.Set<Adiantamento>().AddOrUpdate(adiantamento);
            db.SaveChanges();           
            var texto = "<p>Seu adiantamento foi aprovado &nbsp;" + adiantamento.UserId + "&nbsp;,aguarde setor financeiro realizar a tranferencia bancaria, tambem será notificado.</p></br>";
            var emailuser = db_.Users.Where(user => user.UserName == adiantamento.UserId).Select(user => user.Email).SingleOrDefault();
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(emailuser, texto, "Solicitação de adiantamento");
            var message = "Solicitação aprovada com sucesso, o setor financeiro foi notificado sobre a solicitação para realizar o deposito.";
            //notifica financeiro
            await AprovarNotificaFinanceiro(id);
            return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> AprovarNotificaFinanceiro(int id)
        {
            Adiantamento adiantamento = db.Adiantamento.Find(id);
            Aprovador aprovador = db.Aprovador.Where(a=>a.AprovadorId == adiantamento.AprovadorId).Single();
            Financeiro financeiro = db.Financeiro.Where(f => f.EmpresaId == aprovador.EmpresaId).FirstOrDefault();
            var link = "http://192.168.0.25:8083/Adiantamento/Financeiro";
            var texto = "<p>Funcionário: &nbsp;" + adiantamento.UserId + "&nbsp;, solicitou um valor para adiantamento que foi aprovado pelo gestor responsavel(+"+aprovador.ArpovName+ "+), para visualizar a solicitação e os dados bancarios acesse o sistema através do link.</p></br><p><a href='" + link + "'>" + link + "</p>";
            var emailuser = db_.Users.Where(user => user.Id == financeiro.UserId).Select(user => user.Email).SingleOrDefault();
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(emailuser, texto, "Solicitação de adiantamento");
            var message = "Solicitação aprovada com sucesso, o setor financeiro foi notificado sobre a solicitação para realizar o deposito.";
            return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> ConfirmaPgt(int id)
        {
            Adiantamento adiantamento = db.Adiantamento.Find(id);
            adiantamento.Situacao = "Pagamento realizado utilizado";
            db.Entry(adiantamento).State = EntityState.Detached;
            db.Set<Adiantamento>().AddOrUpdate(adiantamento);
            db.SaveChanges();
            var texto = "<p>Seu adiantamento foi pago &nbsp;" + adiantamento.UserId + "&nbsp;,aguarde o final de sua viagem para fazer a criação do Relatório de Reembolso de viagem - RDV no mesmo irá associar ao adiantamento que foi realizado.</p></br>";
            var emailuser = db_.Users.Where(user => user.UserName == adiantamento.UserId).Select(user => user.Email).SingleOrDefault();
            EmailService emailService = new EmailService();
            var message = "Confirmado, requerente foi notificado";
            await emailService.SendEmailAsync(emailuser, texto, "Adiantamento Pago");
            return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FinanceiroReport(int id)
        {
            ContextRDV db = new ContextRDV();            
            ReportDocument rd = new ReportDocument();
            //Directory.CreateDirectory(@"F:\Projetos\VAMTEC\IntraNet\IntraNet.Mod.RDV\Report\" + id);
            rd.Load(Path.Combine(Server.MapPath(@"~/Report/"), "Adiantamento.rpt"));
            ReportDocument report = new ReportDocument();

            var Adiantamento = (from d in db.Adiantamento
                             select new
                             {
                                 d.AdiantamentoId,
                                 d.UserId,
                                 d.solicitaempresa.Fornecedor,
                                 d.SolicitadoEm,
                                 d.DataPrevista,
                                 d.Situacao,
                                 d.AdiantamentoValor,
                                 d.AprovadorId,
                                 d.DadosBancariosId,
                                 d.dadosbancarios.Banco,
                                 d.dadosbancarios.Agencia,
                                 d.dadosbancarios.Dv,
                                 d.dadosbancarios.ContaCorrente,
                                 d.dadosbancarios.Cpf,
                                 d.aprovador.ArpovName
                             }).Where(d => d.AdiantamentoId == id).ToList();
            rd.SetDatabaseLogon("sa", "sa", "localhost", "data_RDV");
            rd.SetDataSource(Adiantamento);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            //rd.ExportToDisk(ExportFormatType.PortableDocFormat, "~/Report/"+ id+"");
            try
            {

                var situacao = db.Adiantamento.Where(r => r.AdiantamentoId == id).Select(r => r.Situacao).SingleOrDefault();
                if (situacao == "Aprovado" || situacao == "Pagamento realizado utilizado" || situacao == "Pagamento realizado")
                {
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "Relatorio_ID_"+id+".pdf");
                }
                else
                {
                    return Content(@"<script language='javascript' type='text/javascript'>
                     alert('Só é prmitido emitir quando o adiantamento estiver aprovado pelo gestor do funcionário!');window.history.go(-1);</script>");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("erro: " + e);
                throw;
            }
        }
    }
}