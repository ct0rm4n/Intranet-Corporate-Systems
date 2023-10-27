using AutoMapper;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities.SGR;
using IntraNet.Mod.SGR.Models.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntraNet.Mod.SGR.Controllers
{
    public class AssuntoController : Controller
    {
        public ContextSGR db = new ContextSGR();
        public LogController logCFG = new LogController();
        // GET: Assunto
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Delete(int Id)
        {
            string msg;
            var errors = new List<string>();
            
            string credencial = User.Identity.GetUserName();
            Assunto Assunto = db.Assunto.Where(a => a.AssuntoId == Id).Single();
            var userreuniao = db.userreuniao.Where(t => t.ReuniaoId == Assunto.ReuniaoId && t.UserName == credencial).SingleOrDefault();
            try
            {

                //compara o perfil do usuário
                if (userreuniao.Perfil == "Moderador")
                {
                    Assunto.Delete = true;
                    if (ModelState.IsValid)
                    {
                        Domain.Entities.SGR.Assunto AssuntoDomain = Mapper.Map<Assunto>(Assunto);
                        db.Entry(Assunto).State = EntityState.Modified;
                        db.SaveChanges();
                        msg = "O assunto foi removido com sucesso";
                        //AssuntoOriginal = db.Assunto.Where(a => a.AssuntoId == Assunto.AssuntoId).FirstOrDefault();
                        logCFG.Log("Reunião:" + db.reuniao.Find(Assunto.ReuniaoId).Nome + "-ID:" + AssuntoDomain.ReuniaoId + " - Exclusao de assunto |Por:" + credencial);
                        return Json(new { success = true, message = msg, ReuniaoId = Assunto.ReuniaoId }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        foreach (var modelStateVal in ViewData.ModelState.Values)
                        {
                            errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                        }
                    }
                }
                else
                {
                    errors.Add("Voce não possui acesso para realizar esta alteração");
                }
            }
            catch (Exception ex)
            {                
                errors.Add("Error occured:" + ex.Message);
                logCFG.Log("Reunião:" + db.reuniao.Find(Assunto.ReuniaoId).Nome + "-ID:" + Assunto.ReuniaoId +"| Por:" + credencial + " - Ocorreu uma tentativa de Excluir o assunto  -|Erro:" + errors);
                return Json(new { success = false, message = errors, ReuniaoId = Assunto.ReuniaoId });
            }
            Assunto = db.Assunto.Where(a => a.AssuntoId == Assunto.AssuntoId).FirstOrDefault();
            logCFG.Log("Reunião:" + db.reuniao.Find(Assunto.ReuniaoId).Nome + "-ID:" + Assunto.ReuniaoId + "| Por:" + credencial + "  - Ocorreu uma tentativa de Excluir o assunto -|Erro:" + errors);
            return Json(new { success = false, message = errors, ReuniaoId = Assunto.ReuniaoId });
        }

        public ActionResult Create(int id)
        {
            ViewBag.ReuniaoId = id;
            ViewBag.QuemInseriu = User.Identity.Name;
            return View();
        }
        public ActionResult Edit(int id)
        {
            
            ViewBag.QuemInseriu = User.Identity.Name;
            Assunto assunto = db.Assunto.Where(a => a.AssuntoId == id).SingleOrDefault();
            ViewBag.ReuniaoId = assunto.ReuniaoId;
            AssuntoViewModel AssuntoViewModel = Mapper.Map< Assunto, AssuntoViewModel>(assunto);
            return View(AssuntoViewModel);
        }
        [HttpPost]
        public JsonResult EditAssunto(AssuntoViewModel model)
        {
            string msg;
            var errors = new List<string>();
            var AssuntoOriginal = db.Assunto.Where(a => a.AssuntoId == model.AssuntoId).FirstOrDefault();
            string credencial = User.Identity.GetUserName();
            var userreuniao = db.userreuniao.Where(t => t.ReuniaoId == model.ReuniaoId && t.UserName == credencial).SingleOrDefault();
            try
            {

                model.Delete = false;
                if (ModelState.IsValid && userreuniao.Perfil == "Moderador")
                {
                    Domain.Entities.SGR.Assunto AssuntoDomain = Mapper.Map<AssuntoViewModel, Assunto>(model);
                    db.Entry(AssuntoOriginal).State = EntityState.Detached;
                    db.Entry(AssuntoDomain).State = EntityState.Modified;
                    db.SaveChanges();
                    msg = "O assunto foi alterado com sucesso";
                    //AssuntoOriginal = db.Assunto.Where(a => a.AssuntoId == model.AssuntoId).FirstOrDefault();
                    //logCFG.Log("Reunião:" + db.reuniao.Find(AssuntoOriginal.ReuniaoId).Nome + "-ID:" + AssuntoDomain.ReuniaoId + "|Por:" + credencial + " - Modificacao de assunto, de:" + AssuntoOriginal.DescricaoAs + "| para: " + AssuntoDomain.DescricaoAs );
                    return Json(new { success = true, message = msg, ReuniaoId = model.ReuniaoId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    foreach (var modelStateVal in ViewData.ModelState.Values)
                    {
                        errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                    }
                    if (userreuniao.Perfil != "Moderador")
                    {
                        errors.Add("Você não possui privilegio para eecutar essa ação.");
                    }
                }
            }
            catch (Exception ex)
            {
                // AssuntoOriginal = db.Assunto.Where(a => a.AssuntoId == model.AssuntoId).FirstOrDefault();
                errors.Add("Error occured:" + ex.Message);
                //logCFG.Log("Reunião:" + db.reuniao.Find(AssuntoOriginal.ReuniaoId).Nome + "-ID:" + AssuntoOriginal.ReuniaoId + "|Por:" + credencial + " - Ocorreu uma tentativa de Modificacao de assunto de:" + AssuntoOriginal.DescricaoAs + "| para: " + model.DescricaoAs + "-|Erro:"+ errors);
                return Json(new { success = false, message = errors, ReuniaoId = model.ReuniaoId });
            }
            // AssuntoOriginal = db.Assunto.Where(a => a.AssuntoId == model.AssuntoId).FirstOrDefault();
            //logCFG.Log("Reunião:" + db.reuniao.Find(AssuntoOriginal.ReuniaoId).Nome + "-ID:" + AssuntoOriginal.ReuniaoId + "|Por:" + credencial+" - Ocorreu uma tentativa de Modificacao de assunto de:" + AssuntoOriginal.DescricaoAs + "| para: " + model.DescricaoAs + "|Por:" + credencial+"-|Erro:"+ errors);
            return Json(new { success = false, message = errors, ReuniaoId = model.ReuniaoId });
        }
        [HttpPost]
        public JsonResult Create(AssuntoViewModel model)
        {
            string msg;
            var errors = new List<string>();
            string credencial = User.Identity.GetUserName();            
            var userreuniao = db.userreuniao.Where(t => t.ReuniaoId == model.ReuniaoId && t.UserName == credencial).SingleOrDefault();
            model.Delete = false;
            model.InseridoEm = DateTime.UtcNow;
            model.QuemInseriu = credencial;
            if (ModelState.IsValid && userreuniao.Perfil == "Moderador")
            {
                try
                {
                    Assunto AssuntoDomain = Mapper.Map<AssuntoViewModel, Assunto>(model);
                    db.Assunto.Add(AssuntoDomain);
                    db.SaveChanges();
                    msg = "O assunto foi cadastrado com sucesso";
                    logCFG.Log("Reunião:" + db.reuniao.Find(model.ReuniaoId).Nome + "-ID:" + AssuntoDomain.ReuniaoId + "|Por:" + credencial + " - Assunto cadastrado:" + AssuntoDomain.DescricaoAs);
                    return Json(new { success = true, message = msg, ReuniaoId = model.ReuniaoId }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {

                    errors.Add("Error occured:" + ex.Message);
                    logCFG.Log("Reunião:" + db.reuniao.Find(model.ReuniaoId).Nome + "-ID:" + model.ReuniaoId + "|Por:" + credencial + " - Ocorreu uma tentativa fala de inserir assunto:" + model.DescricaoAs + "-|Erro:" + errors);
                    return Json(new { success = false, message = errors, ReuniaoId = model.ReuniaoId });
                }                
            }
            else
            {
                if (userreuniao.Perfil != "Moderador")
                {
                    errors.Add("Você não possui privilegio para eecutar essa ação.");
                }
                foreach (var modelStateVal in ViewData.ModelState.Values)
                {
                    errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                }
            }
                     
            logCFG.Log("Reunião:" + db.reuniao.Find(model.ReuniaoId).Nome + "-ID:" + model.ReuniaoId + "|Por:" + credencial + " - Ocorreu uma tentativa fala de inserir assunto:" + model.DescricaoAs +"-|Erro:" + errors);
            return Json(new { success = false, message = errors, ReuniaoId = model.ReuniaoId });
        }


        [HttpGet]
        public ActionResult ListaAssunto(int ID)
        {
            List<string> d = new List<string>();
            foreach (var cat in db.Assunto.Where(r=>r.ReuniaoId==ID).ToList())
            {
                d.Add(cat.DescricaoAs);
            }
            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }
        //subgrid
        public JsonResult GetAssuntosReuniao(int Id, string sidx, string sort, int page, int rows)
        {
            
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var ReuniaoList = (from u in db.Assunto.Where(usr => usr.ReuniaoId == Id && usr.Delete == false)
                               join r in db.reuniao on u.ReuniaoId equals r.ReuniaoId
                               from reu in db.reuniao.DefaultIfEmpty().Where(usr => usr.ReuniaoId == Id)
                               select new
                               {
                                   u.AssuntoId,
                                   u.Situacao,
                                   u.ReuniaoId,
                                   u.DescricaoAs,
                                   u.QuemInseriu,
                                   u.InseridoEm,
                                   u.Delete,
                               });

            int totalRecords = ReuniaoList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

            if (sort.ToUpper() == "DESC")
            {
                ReuniaoList = ReuniaoList.OrderByDescending(t => t.AssuntoId);
                ReuniaoList = ReuniaoList.Skip(pageIndex * pageSize).Take(pageSize);
            }

            else
            {
                ReuniaoList = ReuniaoList.OrderBy(t => t.AssuntoId);
                ReuniaoList = ReuniaoList.Skip(pageIndex * pageSize).Take(pageSize);
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = ReuniaoList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }


    }
}