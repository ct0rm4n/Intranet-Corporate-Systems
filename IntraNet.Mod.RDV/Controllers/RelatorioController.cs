using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using IntraNet.Data.Context;
using System.Data;
using IntraNet.Domain.Entities;
using IntraNet.Mod.RDV.Models.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using CrystalDecisions.ReportAppServer.DataDefModel;
using IntraNet.Data.Repositories;
using IntraNet.Security.ContextIdentity;
using IntraNet.Security.Filters;
using Microsoft.Ajax.Utilities;
using System.Web.Helpers;
using System.Collections;

namespace IntraNet.Mod.RDV.Controllers
{
    [Authorize]
    public class RelatorioController : Controller
    {
        private RelatorioRepository _rep = new RelatorioRepository();

        // GET: Relatorio
        public ActionResult Index()
        {
            string iduser = User.Identity.GetUserId();
            return View(_rep.GetAll().Where(r => r.UserId == iduser).OrderByDescending(r => r.RelatorioId).ToList());
        }
        public ActionResult TotalDevolucao(int id)
        {
            ContextRDV db = new ContextRDV();
            Relatorio relat = db.Relatorio.Where(r => r.RelatorioId == id).SingleOrDefault();
            decimal TotalDesp =00;
            if(relat.AdiantamentoValor <=0 && db.Despesas.Where(d => d.RelatorioId == id).Select(d => d.Valor).Count() > 0)
            {
                ViewBag.Devolucao = db.Despesas.Where(d => d.RelatorioId == id).Select(d => d.Valor).Count();
            }
            else if(db.Despesas.Where(d => d.RelatorioId == id).Select(d => d.Valor).Count()>0) {
                TotalDesp = db.Despesas.Where(d => d.RelatorioId == id).Select(d => d.Valor).Sum();
                ViewBag.Devolucao = TotalDesp - (relat.AdiantamentoValor ??00);
            }
            
            
            return View();
        }
        [ClaimsAuthorize("Aprovador", "VamtecGroup")]
        public ActionResult Aprovador()
        {
            ContextRDV db = new ContextRDV();
            string iduser = User.Identity.GetUserId();
            string username = User.Identity.GetUserName();
            return View(db.Relatorio.Include(r => r.aprovador).Include(r => r.empresa).Where(r => r.aprovador.UserId == iduser).OrderByDescending(r=>r.RelatorioId).ToList());
        }

        [ClaimsAuthorize("Moderador", "VamtecGroup")]
        public ActionResult Moderador()
        {
            ContextRDV db = new ContextRDV();
            string iduser = User.Identity.GetUserId();
            string username = User.Identity.GetUserName();            
            return View(db.Relatorio.Include(r => r.aprovador).Include(r => r.empresa).OrderByDescending(r => r.RelatorioId).ToList());
        }

        [HttpGet]
        public ActionResult ViewAprov(int id)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            ContextRDV db = new ContextRDV();
            Relatorio relatorio = _rep.RecuperarPorID(id);
            if (relatorio == null)
            {
                return HttpNotFound();
            }
            //caluclo do total de despesas
            var total = db.Despesas.Where(i => i.RelatorioId == id);
            decimal totals = total.Select(s => s.Valor).ToList().Sum();
            ViewBag.Total = totals;
            decimal areceber = totals - (relatorio.AdiantamentoValor ?? 00);

            ViewBag.AReceber = areceber;
            //pega usuaruis e dados
            var nmuser = dbContext.Users.Find(relatorio.UserId).UserName;
            var iduser = dbContext.Users.Find(relatorio.UserId).Id;
            ViewBag.UserId = iduser;
            ViewBag.UserName = nmuser;
            ViewBag.AprovadorId = new SelectList
            (
                db.Aprovador.ToList(),
                "AprovadorId",
                "ArpovName"
            );

            ViewBag.AdiantamentoId = new SelectList(db.Adiantamento.Where(i => i.Situacao == "Aprovado" && i.UserId == relatorio.UserName).ToList(),
                "AdiantamentoId",
                "AdiantamentoValor");
            //ViewBag.Aprovador = new SelectList(identity.NameClaimType, "ClaimTypes", "ClaimValue");
            var solicitaempresa = db.SolicitaEmpresas.Where(i => i.UserId == iduser);
            var emps = from s in solicitaempresa
                       join j in db.Empresa on s.EmpresaId equals j.EmpresaId
                       select new
                       {
                           s.EmpresaId,
                           j.RazaoSocial
                       };
            ViewBag.EmpresaId = new SelectList
            (
                emps.ToList(),
                "EmpresaId",
                "RazaoSocial"
            );
            ViewBag.DadosBancariosId = new SelectList(db.DadosBancarios.Where(i => i.UserId == nmuser), "DadosBancariosId", "Banco");
            ViewBag.RelatorioId = relatorio.RelatorioId;
            var RelatorioDomain = Mapper.Map<Relatorio, RelatorioViewModel>(relatorio);
            return View(RelatorioDomain);
        }
        //[ClaimsAuthorize("Aprovador", "VamtecGroup")]
        public ActionResult Revisao(int id)
        {
            ContextRDV db = new ContextRDV();
            //string iduser = User.Identity.GetUserId();
            return View();
        }
        [ClaimsAuthorize("Financeiro", "VamtecGroup")]
        public ActionResult Financeiro()
        {
            ContextRDV db = new ContextRDV();
            string iduser = User.Identity.GetUserId();
            var local = db.Financeiro.Where(f => f.UserId == iduser).Select(f => f.EmpresaId).Single();
            return View(db.Relatorio.Include(r => r.aprovador).Include(r => r.empresa).Where(r => r.Situacao == "Aprovado" && r.EmpresaId == local || r.Situacao == "Pendente Aprovacao" && r.EmpresaId == local || r.Situacao == "Pendente Revisao" && r.EmpresaId == local).OrderByDescending(r => r.RelatorioId).ToList());
        }
        [HttpPost]
        [HandleError]
        public async Task<ActionResult> EnviarRevisao(int id, string texto, string userid)
        {
            ApplicationDbContext _db = new ApplicationDbContext();
            var msg = "";
            try
            {
                //armazena data do envio
                DateTime time = DateTime.Now;
                string inserirdoem = time.ToString("dd/MM/yyyy HH:mm:ss");
                var link = "http://192.168.0.21:8082/Relatorio/" + id + "";
                texto = texto + "<br/><p>Para acessar o relatório:<a href='" + link + "'>" + link + "</p>";
                var emailuser = _db.Users.Find(userid).Email;
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(emailuser, texto, "Notificação de Revisão de relatório");
                msg = "Notificação encamihada com sucesso!";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return Json(new { Message = msg, Id = id });
        }
        // GET: Relatorio/Details/5
        public ActionResult Details(int id)
        {
            Relatorio relatorio = _rep.RecuperarPorID(id);
            if (relatorio == null)
            {
                return HttpNotFound();
            }
            return View(relatorio);
        }
        public ActionResult exportReport(int id)
        {
            ContextRDV db = new ContextRDV();
            DespesaRepository _rep_desp = new DespesaRepository();
            ReportDocument rd = new ReportDocument();
            //Directory.CreateDirectory(@"F:\Projetos\VAMTEC\IntraNet\IntraNet.Mod.RDV\Report\" + id);
            rd.Load(Path.Combine(Server.MapPath("~/Report/"), "Exemplo.rpt"));
            ReportDocument report = new ReportDocument();

            var despesas = (from r in db.Relatorio
                            join d in db.Despesas on r.RelatorioId equals d.RelatorioId
                            select new
                            {
                                r.RelatorioId,
                                r.Motivo,
                                r.Saida,
                                r.Retorno,
                                Observacoes = r.Observacoes ?? "0",
                                d.Valor,
                                d.DespesasId,
                                d.Descricao,
                                Observacao = d.Observacao ?? "0"
                            }).Where(r => r.RelatorioId == id).ToList();

            rd.SetDatabaseLogon("sa", "sa", "localhost", "data_RDV");
            rd.SetDataSource(despesas);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            //rd.ExportToDisk(ExportFormatType.PortableDocFormat, "~/Report/"+ id+"");
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);

                return File(stream, "application/pdf", "Relatorio.pdf");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("erro: " + e);
                throw;
            }
        }

        public ActionResult UserReport(int id)
        {
            ContextRDV db = new ContextRDV();
            DespesaRepository _rep_desp = new DespesaRepository();
            ReportDocument rd = new ReportDocument();
            //Directory.CreateDirectory(@"F:\Projetos\VAMTEC\IntraNet\IntraNet.Mod.RDV\Report\" + id);
            rd.Load(Path.Combine(Server.MapPath(@"~/Report/"), "UserReport.rpt"));
            ReportDocument report = new ReportDocument();

            var relatroio = (from d in db.Despesas
                             join r in db.Relatorio on d.RelatorioId equals r.RelatorioId
                             //from e in db.Empresa.Where(e => e.EmpresaId == r.EmpresaId).Select(n => n.RazaoSocial)
                             select new
                             {
                                 d.RelatorioId,
                                 r.UserName,
                                 r.Motivo,
                                 r.Saida,
                                 r.Retorno,
                                 r.Observacoes,
                                 Adiantamento = (decimal?)r.AdiantamentoValor ?? 00,
                                 //r.empresa.RazaoSocial,
                                 d.Descricao,
                                 d.Valor,
                                 d.Observacao
                             }).Where(d => d.RelatorioId == id).ToList();
            rd.SetDatabaseLogon("sa", "sa", "localhost", "data_RDV");
            rd.SetDataSource(relatroio);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            //rd.ExportToDisk(ExportFormatType.PortableDocFormat, "~/Report/"+ id+"");
            try
            {
                
                var situacao = db.Relatorio.Where(r => r.RelatorioId == id).Select(r => r.Situacao).SingleOrDefault();
                if ( situacao == "Aprovado" || situacao == "Pendente Aprovacao" || situacao =="Finalizado") {
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "Relatorio_ID_"+id+".pdf");
                }else
                {
                    return Content(@"<script language='javascript' type='text/javascript'>
                     alert('Só é prmitido emitir quando o relatório estiver aprovado!');window.history.go(-1);</script>");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("erro: " + e);
                throw;
            }
        }
        // GET: Relatorio/Create
        public ActionResult Create()
        {
            ContextRDV db = new ContextRDV();
            string nmuser = User.Identity.GetUserName();
            string iduser = User.Identity.GetUserId();
            ViewBag.UserId = iduser;
            ViewBag.UserName = nmuser;
            ViewBag.DadosBancariosId = new SelectList(db.DadosBancarios.Where(i => i.UserId == nmuser), "DadosBancariosId", "Banco");
            DateTime time = DateTime.Now;
            ViewBag.Data = time.ToString("dd-MM-yyyy");

            //var identity = (ClaimsIdentity)User.Identity;
            //IEnumerable<Claim> claims = identity.Claims;
            //var name = identity.Claims.Where(c => c.Type == "Aprovador").SingleOrDefault();

            ViewBag.AprovadorId = new SelectList
            (
                db.Aprovador.ToList(),
                "AprovadorId",
                "ArpovName"
            );
            //ViewBag.Aprovador = new SelectList(identity.NameClaimType, "ClaimTypes", "ClaimValue");
            var solicitaempresa = db.SolicitaEmpresas.Where(i => i.UserId == iduser);
            var emps = from s in solicitaempresa
                       join j in db.Empresa on s.EmpresaId equals j.EmpresaId
                       select new
                       {
                           s.EmpresaId,
                           j.RazaoSocial
                       };
            ViewBag.EmpresaId = new SelectList
                (
                    emps.ToList(),
                    "EmpresaId",
                    "RazaoSocial"
                );
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateRelat(RelatorioViewModel relatorio)
        {
            ContextRDV db = new ContextRDV();
            var msg = "";
            var inserido = false;
            relatorio.Criacao = DateTime.Now;
            relatorio.Situacao = "Incompleto";
            var RelatorioDomain = Mapper.Map<RelatorioViewModel, Relatorio>(relatorio);
            if (ModelState.IsValid && relatorio.Motivo.Length > 9 )
            {
                                  
                msg = "Aguarde para continuar, logo após sera necessario preencher o restante do relatorio...";
                db.Relatorio.Add(RelatorioDomain);
                db.SaveChanges();
                inserido = true;
                return Json(new { Message = msg, Id = RelatorioDomain.RelatorioId, inserido = inserido, isRedirect = false });
                               
            }
            else
            {
                var errors = new List<string>();
                foreach (var modelStateVal in ViewData.ModelState.Values)
                {
                    errors.AddRange(modelStateVal.Errors.Select(error => error.ErrorMessage + "</br>"));
                }
                if (relatorio.Saida > relatorio.Retorno) { errors.Add("Datas - dia de retorno não poderá ser menor que a data de saída" + "</br>"); }
                inserido = false;
                return Json(new { Message = errors, Id = RelatorioDomain.RelatorioId, inserido = false, isRedirect = false });
            }
            //return false;
            //return Json(new { Message = msg, Id = RelatorioDomain.RelatorioId, inserido = inserido, isRedirect = false });
        }
        // GET: Relatorio/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            ContextRDV db = new ContextRDV();
            Relatorio relatorio = _rep.RecuperarPorID(id);
            if (relatorio.Situacao == "Pendente Aprovacao" || relatorio.Situacao == "Aprovado")
            {
                return Content(@"<script language='javascript' type='text/javascript'>
                     alert('Não é possivel editar um relatório que ja esta em faze de aprovação ou aprovado!');window.history.go(-1);</script>");
            }
            //caluclo do total de despesas
            var total = db.Despesas.Where(i => i.RelatorioId == id);
            decimal totals = total.Select(s => s.Valor).ToList().Sum();
            ViewBag.Total = totals;
            ViewBag.Situacao = relatorio.Situacao.ToString();
            decimal areceber = totals - (relatorio.AdiantamentoValor ?? 00);

            ViewBag.AReceber = areceber;
            //pega usuaruis e dados
            var nmuser = dbContext.Users.Find(relatorio.UserId).UserName;
            var iduser = dbContext.Users.Find(relatorio.UserId).Id;
            var setor_user = dbContext.Users.Find(relatorio.UserId).Setor;
            ViewBag.UserId = iduser;
            ViewBag.UserName = nmuser;

            ViewBag.AprovadorId = new SelectList
            (
                db.Aprovador.Where(ap=>ap.EmpresaId == relatorio.EmpresaId && ap.SetorId == setor_user.SetorId).ToList(),
                "AprovadorId",
                "ArpovName"
            );
            ViewBag.AdiantamentoId = new SelectList
            (
                db.Adiantamento.Where(i=>i.Situacao=="Aprovado" && i.UserId == relatorio.UserName).ToList(),
                "AdiantamentoId",
                "AdiantamentoValor"
            );
            //ViewBag.Aprovador = new SelectList(identity.NameClaimType, "ClaimTypes", "ClaimValue");
            var solicitaempresa = db.SolicitaEmpresas.Where(i => i.UserId == iduser);
            var emps = from s in solicitaempresa
                       join j in db.Empresa on s.EmpresaId equals j.EmpresaId
                       select new
                       {
                           s.EmpresaId,
                           j.RazaoSocial
                       };
            ViewBag.EmpresaId = new SelectList
            (
                emps.ToList(),
                "EmpresaId",
                "RazaoSocial"
            );
            ViewBag.DadosBancariosId = new SelectList(db.DadosBancarios.Where(i => i.UserId == nmuser), "DadosBancariosId", "Banco", relatorio.DadosBancariosId);
            ViewBag.RelatorioId = relatorio.RelatorioId;
            var RelatorioDomain = Mapper.Map<Relatorio, RelatorioViewModel>(relatorio);
            return View(RelatorioDomain);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Relatorio relatorio)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            ContextRDV db = new ContextRDV();
            var msg = "";
            relatorio.Criacao = db.Relatorio.Where(r => r.RelatorioId == relatorio.RelatorioId).Select(r => r.Criacao).SingleOrDefault();
            relatorio.Situacao = "Pendente Revisao";
            relatorio.Alteracao = DateTime.Now;
            var inserido = false;
            //var RelatorioDomain = Mapper.Map<RelatorioViewModel, Relatorio>(relatorio);
            var somarateio = 0;
            if (db.RateioItems.Where(r => r.RelatorioId == relatorio.RelatorioId).Count()!=0)
            {
                somarateio = db.RateioItems.Where(r => r.RelatorioId == relatorio.RelatorioId).Select(r => r.Porcentagem).Sum();
            }
            if (relatorio.AdiantamentoId != null&& relatorio.AdiantamentoId>0) {                relatorio.AdiantamentoValor = db.Adiantamento.Where(i => i.AdiantamentoId == relatorio.AdiantamentoId).Select(i=>i.AdiantamentoValor).SingleOrDefault();                    }
            if (ModelState.IsValid && somarateio == 100)
            {                
                _rep.Alterar(relatorio);
                var secretarioemail = dbContext.Users.Where(u => u.UserName == "cinthia.almeida").Select(u=>u.Email).SingleOrDefault();
                var aprovadorid = db.Aprovador.Find(relatorio.AprovadorId);
                var aprovadoremail = dbContext.Users.Find(aprovadorid.UserId).Email;
                msg = "Um novo relatório foi criado por: " + relatorio.UserName + ", e seu gestor aprovador é "+ aprovadoremail + " para revisar o relatório segue o link:  <a href='" + "http://192.168.0.21:8082/Relatorio/ViewAprov/" + relatorio.RelatorioId + "'>http://192.168.0.21:8082/Relatorio/ViewAprov/" + relatorio.RelatorioId + "</a>";
                EmailService emailServiceA = new EmailService();
                await emailServiceA.SendEmailAsync(secretarioemail, string.Concat(msg), "Um Novo Relatório foi Criado");
                var msgAp = "Um novo relatório foi criado por: " + relatorio.UserName + ", e você é gestor aprovador, o mesmo será revisado e em seguida poderá realizar a aprovação do relatório através do link:  <a href='" + "http://192.168.0.21:8082/Relatorio/Aprovador" + relatorio.RelatorioId + "'>http://192.168.0.21:8082/Relatorio/Aprovador" + relatorio.RelatorioId + "</a>";
                await emailServiceA.SendEmailAsync(aprovadoremail, string.Concat(msgAp), "Um Novo Relatório foi Criado");
                msg = "Relatório atualizado com sucesso, aguarde avaliação do Aprovador";
                inserido = true;
                return Json(new { Message = msg, Id = relatorio.RelatorioId, inserido = inserido });
            }
            else
            {
                var errors = new List<string>();
                foreach (var modelStateVal in ViewData.ModelState.Values)
                {
                    errors.AddRange(modelStateVal.Errors.Select(error => error.ErrorMessage + "</br>"));
                }
                if (relatorio.Saida > relatorio.Retorno) { errors.Add("Datas - dia de retorno não poderá ser menor que a data de saída" + "</br>"); }

                if (db.Despesas.Where(i => i.RelatorioId == relatorio.RelatorioId).Count() < 1) { errors.Add("Despesas - É obrigatorio inserir as despesas" + "</br>"); }

                if (somarateio < 100) { errors.Add("Rateio - O rateio ainda não completou 100%"+"</br>"); }
                
                if (relatorio.DadosBancariosId == 0 || relatorio.DadosBancariosId ==null) { errors.Add("Dados Bancarios - Informe os dados para tramites financeiros"+"</br>"); }
                inserido = false;
                return Json(new { Message = errors, Id = relatorio.RelatorioId, inserido = inserido });
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> AprovarRev(int id)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            var msg = "";
            try
            {
                ContextRDV dbs = new ContextRDV();
                Relatorio relatorio = _rep.RecuperarPorID(id);
                relatorio.Situacao = "Pendente Aprovacao";
                _rep.Alterar(relatorio);
                var Moderador_aprovador = dbs.Aprovador.Where(a => a.Moderador == true).Select(a => a.ArpovName).FirstOrDefaultAsync();
                msg = "Relatorio ja foi revisado pelo moderador: " + string.Concat(Moderador_aprovador) + "<br /> , Para acessar o relatório segue o link: <a href='" + "http://192.168.0.21:8082/Relatorio/Edit/" + relatorio.RelatorioId + "'>http://192.168.0.21:8081/Relatorio/Edit/" + relatorio.RelatorioId + "</a>";
                EmailService emailService = new EmailService();
                var useremail = dbContext.Users.Find(relatorio.UserId).Email;
                await emailService.SendEmailAsync(useremail, msg, "Seu relatório ja foi Revisado");
                EmailService emailServiceF = new EmailService();
                msg = "Um novo relatório foi Revisado, ja está disponivel para aprovação para visualizar o relatório segue o link:  <a href='" + "http://192.168.0.21:8082/Relatorio/Aprovador" + "'>http://192.168.0.21:8082/Aprovador" + "</a>";
                //var financeiro = dbContext.Users.Find(dbs.Financeiro.Where(f => f.EmpresaId == relatorio.EmpresaId).Select(r => r.UserId).FirstOrDefault()).Email;
                var id_ap = dbs.Aprovador.Where(a => a.AprovadorId == relatorio.AprovadorId).Select(a => a.UserId).SingleOrDefault();
                var aprovemail = dbContext.Users.Find(id_ap).Email;
                await emailServiceF.SendEmailAsync(aprovemail, string.Concat(msg), "Ha novos relatório pendentes aprovação");
                msg = "Notificado com sucesso!";
            }
            catch (Exception e)
            {
                msg = "Ocorreu um erro, por favor entre em contato com suporte";
                Console.WriteLine(e);
                throw;
            }
            return Json(new { Message = msg, Id = id });
        }
        public async Task<ActionResult> AprovarRelat(int id)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            var msg = "";
            try
            {
                ContextRDV dbs = new ContextRDV();
                Relatorio relatorio = _rep.RecuperarPorID(id);
                relatorio.Situacao = "Aprovado";
                _rep.Alterar(relatorio);
                var Moderador_aprovador = dbs.Aprovador.Where(a => a.EmpresaId == relatorio.EmpresaId).Select(a => a.ArpovName).FirstOrDefaultAsync();
                msg = "Relatorio ja foi aprovad pelo aprovador: " + string.Concat(Moderador_aprovador) + "<br /> , Para acessar o relatório segue o link: <a href='" + "http://192.168.0.21:8082/Relatorio/Edit/" + relatorio.RelatorioId + "'>http://192.168.0.21:8081/Relatorio/Edit/" + relatorio.RelatorioId + "</a>";
                EmailService emailService = new EmailService();
                var useremail = dbContext.Users.Find(relatorio.UserId).Email;
                await emailService.SendEmailAsync(useremail, msg, "Seu relatório ja foi Aprovado");
                EmailService emailServiceF = new EmailService();
                msg = "Um novo relatório foi aprovado, ja está disponivel para reembolso para acessar o sistema segue link:  <a href='" + "http://192.168.0.21:8082/Relatorio/Financeiro" + "'>http://192.168.0.21:8082/Financeiro" + "</a>";
                //var financeiro = dbContext.Users.Find(dbs.Financeiro.Where(f => f.EmpresaId == relatorio.EmpresaId).Select(r => r.UserId).FirstOrDefault()).Email;
                var id_fin = dbs.Financeiro.Where(a => a.EmpresaId == relatorio.EmpresaId).Select(a => a.UserId).SingleOrDefault();
                var finemail = dbContext.Users.Find(id_fin).Email;
                await emailServiceF.SendEmailAsync(finemail, string.Concat(msg), "Ha novos relatório aprovados para");
                msg = "Notificado com sucesso!";
            }
            catch (Exception e)
            {
                msg = "Ocorreu um erro, por favor entre em contato com suporte";
                Console.WriteLine(e);
                throw;
            }
            return Json(new { Message = msg, Id = id });
        }
        // GET: Relatorio/Delete/5
        public ActionResult Delete(int id)
        {
            Relatorio relatorio = _rep.RecuperarPorID(id);
            if (relatorio == null)
            {
                return HttpNotFound();
            }
            return View(relatorio);
        }

        // POST: Relatorio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Relatorio relatorio = _rep.RecuperarPorID(id);
            _rep.Remover(relatorio);
            return RedirectToAction("Index");
        }

        public ActionResult Grafico()
        {            
            ContextRDV db = new ContextRDV();
            string iduser = User.Identity.GetUserId();
            var relatorio = db.Relatorio.Where(r => r.UserId == iduser);

            var relatorio_jan = relatorio.Where(r => r.Retorno >= new DateTime(2018, 1, 1) && r.Retorno <= new DateTime(2018, 1, 31)).Select(r=>r.RelatorioId);
            var despesa_soma_jan = db.Despesas.Where(d => relatorio_jan.Contains(d.RelatorioId));
            decimal soma_jan=0;
            if (despesa_soma_jan.Count()>0) {
                soma_jan = despesa_soma_jan.Select(d => d.Valor).Sum();
            }

            var relatorio_fev = relatorio.Where(r => r.Retorno >= new DateTime(2018, 2, 1) && r.Retorno <= new DateTime(2018, 2, 28)).Select(r => r.RelatorioId);
            var despesa_soma_fev = db.Despesas.Where(d => relatorio_fev.Contains(d.RelatorioId));
            decimal soma_fev = 0;
            if (despesa_soma_fev.Count()>0){
                soma_fev = despesa_soma_fev.Select(d => d.Valor).Sum();
            }

            var relatorio_marc = relatorio.Where(r => r.Retorno >= new DateTime(2018, 3, 1) && r.Retorno <= new DateTime(2018, 3, 31)).Select(r => r.RelatorioId);
            var despesa_soma_marc = db.Despesas.Where(d => relatorio_marc.Contains(d.RelatorioId));
            decimal soma_marc = 0;
            if (despesa_soma_marc.Count()>0){
                soma_marc = despesa_soma_marc.Select(d => d.Valor).Sum();
            }

            var relatorio_abr = relatorio.Where(r => r.Retorno >= new DateTime(2018, 4, 1) && r.Retorno <= new DateTime(2018, 4, 30)).Select(r => r.RelatorioId);
            var despesa_soma_abr = db.Despesas.Where(d => relatorio_abr.Contains(d.RelatorioId));
            decimal soma_abr = 0;
            if (despesa_soma_abr.Count()>0){
                soma_abr = despesa_soma_abr.Select(d => d.Valor).Sum();
            }

            var relatorio_maio = relatorio.Where(r => r.Retorno >= new DateTime(2018, 5, 1) && r.Retorno <= new DateTime(2018, 5, 31)).Select(r => r.RelatorioId);
            var despesa_soma_maio = db.Despesas.Where(d => relatorio_maio.Contains(d.RelatorioId));
            decimal soma_maio = 0;
            if (despesa_soma_maio.Count()>0){
                soma_maio = despesa_soma_maio.Select(d => d.Valor).Sum();
            }

            var relatorio_junho = relatorio.Where(r => r.Retorno >= new DateTime(2018, 6, 1) && r.Retorno <= new DateTime(2018, 6, 30)).Select(r => r.RelatorioId);
            var despesa_soma_junho = db.Despesas.Where(d => relatorio_junho.Contains(d.RelatorioId));
            decimal soma_junho = 0;
            if (despesa_soma_junho.Count() > 0)
            {
                soma_junho = despesa_soma_junho.Select(d => d.Valor).Sum();
            }

            var relatorio_julho = relatorio.Where(r => r.Retorno >= new DateTime(2018, 7, 1) && r.Retorno <= new DateTime(2018, 7, 30)).Select(r => r.RelatorioId);
            var despesa_soma_julho = db.Despesas.Where(d => relatorio_julho.Contains(d.RelatorioId));
            decimal soma_julho = 0;
            if (despesa_soma_julho.Count() > 0)
            {
                soma_julho = despesa_soma_julho.Select(d => d.Valor).Sum();
            }

            var relatorio_ags = relatorio.Where(r => r.Retorno >= new DateTime(2018, 8, 1) && r.Retorno <= new DateTime(2018, 8, 31)).Select(r => r.RelatorioId);
            var despesa_soma_ags = db.Despesas.Where(d => relatorio_ags.Contains(d.RelatorioId));
            decimal soma_ags = 0;
            if (despesa_soma_ags.Count() > 0)
            {
                soma_ags = despesa_soma_ags.Select(d => d.Valor).Sum();
            }

            var relatorio_set = relatorio.Where(r => r.Retorno >= new DateTime(2018, 9, 1) && r.Retorno <= new DateTime(2018, 9, 30)).Select(r => r.RelatorioId);
            var despesa_soma_set = db.Despesas.Where(d => relatorio_set.Contains(d.RelatorioId));
            decimal soma_set = 0;
            if (despesa_soma_set.Count() > 0)
            {
                soma_set = despesa_soma_set.Select(d => d.Valor).Sum();
            }

            var relatorio_out = relatorio.Where(r => r.Retorno >= new DateTime(2018, 10, 1) && r.Retorno <= new DateTime(2018, 10, 31)).Select(r => r.RelatorioId);
            var despesa_soma_out = db.Despesas.Where(d => relatorio_out.Contains(d.RelatorioId));
            decimal soma_out = 0;
            if (despesa_soma_out.Count() > 0)
            {
                soma_out = despesa_soma_out.Select(d => d.Valor).Sum();
            }

            var relatorio_nov = relatorio.Where(r => r.Retorno >= new DateTime(2018, 11, 1) && r.Retorno <= new DateTime(2018, 11, 30)).Select(r => r.RelatorioId);
            var despesa_soma_nov = db.Despesas.Where(d => relatorio_nov.Contains(d.RelatorioId));
            decimal soma_nov = 0;
            if (despesa_soma_nov.Count() > 0)
            {
                soma_nov = despesa_soma_nov.Select(d => d.Valor).Sum();
            }

            var relatorio_dez = relatorio.Where(r => r.Retorno >= new DateTime(2018, 12, 1) && r.Retorno <= new DateTime(2018, 12, 31)).Select(r => r.RelatorioId);
            var despesa_soma_dez = db.Despesas.Where(d => relatorio_dez.Contains(d.RelatorioId));
            decimal soma_dez = 0;
            if (despesa_soma_dez.Count() > 0)
            {
                soma_dez = despesa_soma_dez.Select(d => d.Valor).Sum();
            }
            var bytes = new Chart(width: 700, height: 300)
                .AddTitle("2018")
                .AddLegend("Gastos Mensais")
                .AddSeries(
                    name: "Total", yFields: "teste",                   
                    xValue: new[] { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto","Setembro", "Outubro","Novembro","Desembro" },
                    yValues: new[] { soma_jan.ToString(), soma_fev.ToString(), soma_marc.ToString(), soma_abr.ToString(), soma_junho.ToString(),"","","","","","","" })
                .GetBytes("png");
            return File(bytes, "image/png");
        }

        public ActionResult Pesquisa()
        {
            ViewBag.Situacao = new List<SelectListItem>()
            {
                new SelectListItem {Value = "Todos", Text = "Todos", Selected=true},
                new SelectListItem{Value = "Incompleto", Text = "Incompleto"},
                new SelectListItem{Value = "Pendente Revisao", Text = "Pend. Revisão"},
                new SelectListItem{Value = "Pendente Aprovacao", Text = "Pend. Aprovação"},
                new SelectListItem{Value = "Aprovado", Text = "Aprovado"}
            };
            return View();
        }
        [HttpPost]
        public ActionResult PesquisaRetorno(PesquisaRelatViewModel fd)
        {
            var inserido = true;
            if (ModelState.IsValid)
            {
                string iduser = User.Identity.GetUserId();
                IEnumerable<Relatorio> result = _rep.GetAll().Where(r => r.UserId == iduser && r.Motivo.Contains(fd.PalavraPasse)).OrderByDescending(r => r.RelatorioId).ToList();
                return View(result);
            }
            else
            {
                var errors = new List<string>();
                foreach (var modelStateVal in ViewData.ModelState.Values)
                {
                    errors.AddRange(modelStateVal.Errors.Select(error => error.ErrorMessage + "</br>"));
                }
                inserido = false;
                return Json(new { Message = errors, inserido = inserido });
            }
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
