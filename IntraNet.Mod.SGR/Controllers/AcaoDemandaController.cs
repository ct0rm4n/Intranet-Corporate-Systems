using IntraNet.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using IntraNet.Domain.Entities.SGR;
using IntraNet.Mod.SGR.Models.ViewModel;
using IntraNet.Domain.Entities;
using IntraNet.Security.ContextIdentity;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace IntraNet.Mod.SGR.Controllers
{
    public class AcaoDemandaController : Controller
    {
        private ContextSGR db = new ContextSGR();
        // GET: AcaoDemanda
        public ActionResult Index(int id)
        {
            var adem = db.AcaoDemanda.Where(d => d.DemandaId == id).ToList();
            IEnumerable <AcaoDemandaViewModel> AcaoViewModel = Mapper.Map<IEnumerable< AcaoDemandaViewModel> >(adem);
            return View(AcaoViewModel);
        }
        public JsonResult GetAcaoDemandaSubGrid(int ID, string sidx, string sort, int page, int rows)
        {
            ContextSGR db = new ContextSGR();
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            System.Diagnostics.Debug.Write(ID);
            //var iddem = db.Demandas.Where(i => i.Descricao == ID).Single();
            var AcaoDemandaList = db.AcaoDemanda.Where(t => t.DemandaId == ID && t.Delete!=true).AsEnumerable().Select(
                t => new
                {
                    t.AcaoDemandaId,
                    t.DemandaId,
                    t.ReuniaoId,
                    t.Descricao,
                    t.Feito,
                    t.Data,
                    t.InseridoEm,
                    t.QuemInseriu,
                    t.Demandado,
                    t.Observacao
                });


            int totalRecords = AcaoDemandaList.Count();

            System.Diagnostics.Debug.Write(totalRecords, "Total de linhas");
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);            
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = AcaoDemandaList
            };
            System.Diagnostics.Debug.WriteLine(AcaoDemandaList.ToArray());
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Create(int id)
        {
            IList<AcaoDemanda> ultima_acao = db.AcaoDemanda.Where(d => d.DemandaId == id).ToList();
            //ViewBag.ReuniaoId = db.Demanda.Where(d=>d.DemandaId ==id).Select(d=>d.ReuniaoId);
            ViewBag.InseridoEm = DateTime.UtcNow;
            ViewBag.Data = DateTime.Now;
            ViewBag.DemandaId = id;
            ViewBag.ReuniaoId = db.Demanda.Where(r => r.DemandaId == id).Select(r => r.ReuniaoId).SingleOrDefault();
            return View();
        }

        [HttpPost]
        public JsonResult CreateAcao(AcaoDemandaViewModel acaodemandaview)
        {

            ApplicationDbContext _db = new ApplicationDbContext();
            string message = "";
            acaodemandaview.InseridoEm = DateTime.Now;
            acaodemandaview.Delete = false;
            string credencial = User.Identity.GetUserName();
            acaodemandaview.QuemInseriu = credencial;
            var errors = new List<string>();            
            var userreuniao = db.userreuniao.Where(t => t.ReuniaoId == acaodemandaview.ReuniaoId && t.UserName == credencial).SingleOrDefault();
            var success = 0;            
            if (ModelState.IsValid)
            {
                try
                {
                    AcaoDemanda DemandaDomain = Mapper.Map<AcaoDemanda>(acaodemandaview);
                    db.AcaoDemanda.Add(DemandaDomain);
                    //db.Entry(DemandaDomain).State = EntityState.Modified;
                    db.SaveChanges();
                    message = "O Demanda foi atualizada conferme o ação que foi realizada, sempre a mantenha as demandas em dia.";
                    //logCFG.Log("Reunião:" + db.reuniao.Find(acaodemandaview.ReuniaoId).Nome + "-ID:" + demandaview.ReuniaoId + "|Por:" + credencial + " - Demanda cadastrada:" + db.itemassunto.Find(demandaview.ItemAssuntoId).DescricaoItem + "- Assunto:" + db.Assunto.Find(db.itemassunto.Find(demandaview.ItemAssuntoId).AssuntoId).DescricaoAs + "- Item: " + db.reuniao.Find(demandaview.ReuniaoId).Nome);
                    //var link = "http://localhost:8082/Reuniao/Ata/" + acaodemandaview.ReuniaoId + "";
                    //var texto = "<p>Uma ação foi registrada para voce, sera responsavel do registrar o progresso da mesma e o que foi realizado até o momento:&nbsp;" + demandaview.Oque + " que pertence ao item:&nbsp;" + db.itemassunto.Find(demandaview.ItemAssuntoId).DescricaoItem + "e do assunto" + db.Assunto.Find(db.itemassunto.Find(demandaview.ItemAssuntoId).AssuntoId).DescricaoAs + ", para acessar a ATA da&nbsp;<b>Reunião:&nbsp;" + db.reuniao.Find(demandaview.ReuniaoId).Nome + "</b><a href='" + link + "'>&nbsp;Click aqui</p>";
                    //var idresp = _db.Users.Where(u => u.UserName == demandaview.Demandado).Select(u => u.Id).SingleOrDefault();
                    //var emailresp = _db.Users.Find(idresp).Email;
                    //EmailService emailService = new EmailService();
                    //await emailService.SendEmailAsync(emailresp, texto, "SGR - Nova responsabilidade");
                    success = 1;
                    //PartialViewResult result = PartialView("Index","AcaoDemanda" 2);
                    return Json(new { success = success, message = message, ReuniaoId = acaodemandaview.ReuniaoId }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    success = 0;
                    errors.Add("Ocorreu o erro:" + ex);
                }
                
            }
            else
            {
                success = 0;
                foreach (var modelStateVal in ViewData.ModelState.Values)
                {

                    errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                }
            }
            
            
            return Json(new { success = success, message = message, ReuniaoId = acaodemandaview.ReuniaoId }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Edit(AcaoDemandaViewModel model)
        {
            string msg;
            var errors = new List<string>();            
            var AcaoDemandaOriginal = db.AcaoDemanda.Where(a => a.AcaoDemandaId == model.AcaoDemandaId).FirstOrDefault();
            string credencial = User.Identity.GetUserName();
            var userreuniao = db.userreuniao.Where(t => t.ReuniaoId == model.ReuniaoId && t.UserName == credencial).SingleOrDefault();
            try
            {
                if (ModelState.IsValid && userreuniao.Perfil == "Moderador")
                {
                    model.Delete = false;

                    AcaoDemanda AcaoDemandaDomain = Mapper.Map<AcaoDemandaViewModel, AcaoDemanda>(model);
                    db.Entry(AcaoDemandaOriginal).State = EntityState.Detached;
                    db.Entry(AcaoDemandaDomain).State = EntityState.Modified;
                    db.SaveChanges();
                    msg = "A ação da demanda foi alterada com sucesso";
                    //var link = "http://localhost:8082/Reuniao/Ata/" + model.ReuniaoId + "";
                    //var texto = "<p>Foi alterado um item que você está como responsavel:&nbsp;" + model.DescricaoItem + " que pertence ao assunto:&nbsp;" + db.Assunto.Find(model.AssuntoId).DescricaoAs + ", para acessar a ATA da &nbsp;<b>Reunião:&nbsp;" + db.reuniao.Find(model.ReuniaoId).Nome + "</b><a href='" + link + "'>&nbsp;Click aqui</p>";
                    // var idresp = _db.Users.Where(u => u.UserName == model.Responsavel).Select(u => u.Id).SingleOrDefault();
                    //var emailresp = _db.Users.Find(idresp).Email;
                    // EmailService emailService = new EmailService();
                    //// await emailService.SendEmailAsync(emailresp, texto, "SGR - Alteração de item");
                    //logCFG.Log("Reunião:" + db.reuniao.Find(ItemAssuntoOriginal.ReuniaoId).Nome + "-ID:" + ItemAssuntoDomain.ReuniaoId + "|Por:" + credencial+ " - Modificacao de item de assunto, de:" + ItemAssuntoOriginal.DescricaoItem + "| para: " + ItemAssuntoDomain.DescricaoItem);
                    return Json(new { success = 1, message = msg, ReuniaoId = model.ReuniaoId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (userreuniao.Perfil == "Participante") { errors.Add("Voce não possui acesso para realizar esta alteração"); }
                    foreach (var modelStateVal in ViewData.ModelState.Values)
                    {
                        errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                //ItemAssuntoOriginal = db.itemassunto.Where(a => a.AssuntoId == model.AssuntoId).FirstOrDefault();
                errors.Add("Error occured:" + ex.Message);
                //logCFG.Log("Reunião:" + db.reuniao.Find(AssuntoOriginal.ReuniaoId).Nome + "-ID:" + AssuntoOriginal.ReuniaoId + " - Ocorreu uma tentativa de Modificacao de assunto de:" + AssuntoOriginal.DescricaoAs + "| para: " + model.DescricaoAs + "|Por:" + credencial + "-|Erro:" + errors);
                return Json(new { success = 0, message = errors, ReuniaoId = model.ReuniaoId }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = 0, message = errors, ReuniaoId = model.ReuniaoId }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int id)
        {
            string msg;
            var errors = new List<string>();
            var AcaoDemandaOriginal = db.AcaoDemanda.Where(a => a.AcaoDemandaId == id).FirstOrDefault();
            string credencial = User.Identity.GetUserName();
            var userreuniao = db.userreuniao.Where(t => t.ReuniaoId == AcaoDemandaOriginal.ReuniaoId && t.UserName == credencial).SingleOrDefault();
            try
            {
                if (ModelState.IsValid)
                {
                    AcaoDemandaOriginal.Delete = true;                    
                    db.Entry(AcaoDemandaOriginal).State = EntityState.Detached;
                    db.Entry(AcaoDemandaOriginal).State = EntityState.Modified;
                    db.SaveChanges();
                    msg = "A ação da demanda foi removida com sucesso";
                    //var link = "http://localhost:8082/Reuniao/Ata/" + model.ReuniaoId + "";
                    //var texto = "<p>Foi alterado um item que você está como responsavel:&nbsp;" + model.DescricaoItem + " que pertence ao assunto:&nbsp;" + db.Assunto.Find(model.AssuntoId).DescricaoAs + ", para acessar a ATA da &nbsp;<b>Reunião:&nbsp;" + db.reuniao.Find(model.ReuniaoId).Nome + "</b><a href='" + link + "'>&nbsp;Click aqui</p>";
                    // var idresp = _db.Users.Where(u => u.UserName == model.Responsavel).Select(u => u.Id).SingleOrDefault();
                    //var emailresp = _db.Users.Find(idresp).Email;
                    // EmailService emailService = new EmailService();
                    //// await emailService.SendEmailAsync(emailresp, texto, "SGR - Alteração de item");
                    //logCFG.Log("Reunião:" + db.reuniao.Find(ItemAssuntoOriginal.ReuniaoId).Nome + "-ID:" + ItemAssuntoDomain.ReuniaoId + "|Por:" + credencial+ " - Modificacao de item de assunto, de:" + ItemAssuntoOriginal.DescricaoItem + "| para: " + ItemAssuntoDomain.DescricaoItem);
                    return Json(new { success = 1, message = msg, ReuniaoId = AcaoDemandaOriginal.ReuniaoId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    
                    foreach (var modelStateVal in ViewData.ModelState.Values)
                    {
                        errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                //ItemAssuntoOriginal = db.itemassunto.Where(a => a.AssuntoId == model.AssuntoId).FirstOrDefault();
                errors.Add("Error occured:" + ex.Message);
                //logCFG.Log("Reunião:" + db.reuniao.Find(AssuntoOriginal.ReuniaoId).Nome + "-ID:" + AssuntoOriginal.ReuniaoId + " - Ocorreu uma tentativa de Modificacao de assunto de:" + AssuntoOriginal.DescricaoAs + "| para: " + model.DescricaoAs + "|Por:" + credencial + "-|Erro:" + errors);
                return Json(new { success = 0, message = errors, ReuniaoId = AcaoDemandaOriginal.ReuniaoId }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = 0, message = errors, ReuniaoId = AcaoDemandaOriginal.ReuniaoId }, JsonRequestBehavior.AllowGet);
        }
    }
}