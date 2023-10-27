using IntraNet.Data.Context;
using IntraNet.Security.ContextIdentity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntraNet.Domain.Entities.SGR;
using IntraNet.Security.Models;
using IntraNet.Domain.Entities;
using System.Threading.Tasks;

namespace IntraNet.Mod.SGR.Controllers
{
    public class DamandadoReuController : Controller
    {
        private ApplicationDbContext dbIdentity = new ApplicationDbContext();
        public ContextSGR db = new ContextSGR();
        public LogController logCFG = new LogController();
        // GET: Demanda
        public ActionResult Index()
        {
            
            return View();
        }
        
        public ActionResult ListaDemandados(int ID)
        {
            List<string> demandados = new List<string>();
            foreach (var cat in db.userdemanda.Where(r => r.DemandaId == ID && r.Delete==false).ToList())
            {
                demandados.Add(cat.UserId);
            }

            ViewBag.DemandadosLista = demandados;
            return View();
        }
        public string RetornaDemandadosD(int id)
        {
            //retorna todos os usuários que solicitaram demandas
            string demandados = "";
            //var ids_demandas = db.Demanda.Where(i => i.ItemAssuntoId == id).Select(d => d.DemandaId).ToList();
            //var u_demandado = db.userdemanda.Where(x => ids_demandas.Contains(x.DemandaId)).Select(x => x.UserId).Distinct().ToList();
            var u_demandado = db.userdemanda.Where(x => x.DemandaId == id).Select(x => x.UserId).Distinct().ToList();
            foreach (var item in u_demandado)
            {
                demandados = demandados + item + ", ";
            }

            return demandados;
        }
        [HttpPost]
        public async Task<JsonResult> NotificaDemandadosEmail()
        {
            var demanda_pendentes = db.Demanda.Where(d => d.Situacao == "Aberto").Include(d=>d.reuniao).ToList();
            try
            {
                foreach (var item in demanda_pendentes)
                {
                    if (item.Quando > DateTime.Now)
                    {
                        //logCFG.Log("Reunião:" + db.reuniao.Find(reuniaoview.ReuniaoId).Nome + "-ID:" + reuniaoview.ReuniaoId + "|Por:" + credencial + " - Demanda cadastrada:" + db.itemassunto.Find(demandaview.ItemAssuntoId).DescricaoItem + "- Assunto:" + db.Assunto.Find(db.itemassunto.Find(demandaview.ItemAssuntoId).AssuntoId).DescricaoAs + "- Item: " + db.reuniao.Find(demandaview.ReuniaoId).Nome);
                        var link = "http://localhost:8082/Reuniao/Ata/" + item.ReuniaoId + "";
                        var texto = "<p>Voce está participando de uma demanda na reunião:" + item.reuniao.Nome + ", todo progresso devera ser incluido no sistema</p>&nbsp;<p>Ela está em aberto" + "</b><a href='" + link + "'>&nbsp;Click aqui</p>";
                        var HTML = "<html><body><table id='vertical-2' style='table, th, td {borde:1px solid black;border-collapse: collapse}'>" +
                                   "<caption> Demanda </caption>" +
                                   "<thead>" +
                                   "<tr>" +
                                   "<th> O que? </ th >" +
                                   "<th> Onde? </th>" +

                                   "<th>Como? Porque? </th>" +

                                   "<th> Onde?  Quanto?</th>" +

                                   "<th> Demandados? </th>" +

                                   "<th> Até Quando? </th>" +
                                   "</tr>" +
                                   "</thead>" +

                                   "<tbody>" +

                                   "<tr>" +

                                   "<td> " + item.Oque + " </td>" +
                                   "<td> " + item.Onde + "</td>" +
                                   "<td> <b>Porque:</b>" + item.Porque + "<br/><b>Como:</b>" + item.Como + "</td>" +
                                   "<td> <b>Onde:</b>" + item.Onde + "<br/><b>Quanto:</b>" + item.Quando + "</td>" +
                                   "<td> teste</td>" +
                                   "<td> " + item.Quando + "</td>" +
                                   "</tr>" +

                                   "</tbody >" +

                                   "<tfoot>" +

                                   "<tr>" +

                                   "<td colspan='4'> Historico </td>" +

                                   "</tr>" +

                                   "</tfoot>" +

                                   "</table></body></html>";
                        var Demandados = db.userdemanda.Where(d => d.DemandaId == item.DemandaId).Select(d => d.UserId)
                            .ToList();
                        foreach (var user in Demandados)
                        {
                            var emailuser = dbIdentity.Users.Where(u => u.UserName == user).Select(u => u.Email).SingleOrDefault();
                            EmailService emailService = new EmailService();
                            await emailService.SendEmailAsync(emailuser, HTML, "SGR - Pendencia");
                        }
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //Demandada ja vencida
                        var link = "http://localhost:8082/Reuniao/Ata/" + item.ReuniaoId + "";
                        var texto = "<p>Voce está participando de uma demanda na reunião:" + item.reuniao.Nome + ", todo progresso devera ser incluido no sistema</p>&nbsp;<p>Ela está em aberto" + "</b><a href='" + link + "'>&nbsp;Click aqui</p>";
                        var HTML = "<html><body><table id='vertical-2' style='table, th, td {borde:1px solid black;border-collapse: collapse}'>" +
                                   "<caption> Demanda </caption>" +
                                   "<thead>" +
                                   "<tr>" +
                                   "<th> O que? </ th >" +
                                   "<th> Onde? </th>" +

                                   "<th>Como? Porque? </th>" +

                                   "<th> Onde?  Quanto?</th>" +

                                   "<th> Demandados? </th>" +

                                   "<th> Até Quando? </th>" +
                                   "</tr>" +
                                   "</thead>" +

                                   "<tbody>" +

                                   "<tr>" +

                                   "<td> " + item.Oque + " </td>" +
                                   "<td> " + item.Onde + "</td>" +
                                   "<td> <b>Porque:</b>" + item.Porque + "<br/><b>Como:</b>" + item.Como + "</td>" +
                                   "<td> <b>Onde:</b>" + item.Onde + "<br/><b>Quanto:</b>" + item.Quando + "</td>" +
                                   "<td> teste</td>" +
                                   "<td> " + item.Quando + "</td>" +
                                   "</tr>" +

                                   "</tbody >" +

                                   "<tfoot>" +

                                   "<tr>" +

                                   "<td colspan='4'> Historico </td>" +

                                   "</tr>" +

                                   "</tfoot>" +

                                   "</table></body></html>";
                        var Demandados = db.userdemanda.Where(d => d.DemandaId == item.DemandaId).Select(d => d.UserId)
                            .ToList();
                        foreach (var user in Demandados)
                        {
                            var emailuser = dbIdentity.Users.Where(u => u.UserName == user).Select(u => u.Email).SingleOrDefault();
                            EmailService emailService = new EmailService();
                            await emailService.SendEmailAsync(emailuser, HTML, "SGR - Pendencia");
                        }
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetUsuariosReuList(string searchTerm)
        {
            
            var DemandadoReu_ = dbIdentity.Users.ToList();
            if (searchTerm != null)
            {
                DemandadoReu_ = dbIdentity.Users.Where(x => x.UserName.Contains(searchTerm) || x.Email.Contains(searchTerm)).ToList();
                
            }

            var Resultado = DemandadoReu_.Select(x=>new{
                id = x.Id,
                text = x.UserName,
                email = x.Email,
                img = x.ImagePath
            });
            return Json(Resultado, JsonRequestBehavior.AllowGet);
        }
        // GET: DamandadoReu/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DamandadoReu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DamandadoReu/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DamandadoReu/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DamandadoReu/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DamandadoReu/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DamandadoReu/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
