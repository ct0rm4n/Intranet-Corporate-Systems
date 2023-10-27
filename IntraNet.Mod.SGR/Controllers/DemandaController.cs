using AutoMapper;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities;
using IntraNet.Domain.Entities.SGR;
using IntraNet.Mod.SGR.Models.ViewModel;
using IntraNet.Security.ContextIdentity;

using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;

namespace IntraNet.Mod.SGR.Controllers
{
    public class DemandaController : Controller
    {
        private ApplicationDbContext dbIdentity = new ApplicationDbContext();
        public ContextSGR db = new ContextSGR();
        public LogController logCFG = new LogController();
        public DamandadoReuController demandadoreu = new DamandadoReuController();
        public ContextRDV dbRDV = new ContextRDV();
        // GET: Demanda

        public ActionResult Index(int Id)
        {
            IEnumerable<Demanda> demandas_item =db.Demanda.Where(d => d.ItemAssuntoId == Id && d.Delete==false).ToList();
            IEnumerable<DemandaViewModel> DemandaViewModel = Mapper.Map<IEnumerable<DemandaViewModel>>(demandas_item);
            IEnumerable<UserDemanda> demandados_list;
            foreach (var demanda in demandas_item)
            {
                demandados_list = null;
                if (db.userdemanda.Where(d => d.DemandaId == demanda.DemandaId && demanda.Delete == false).Count()>0)
                {
                    IEnumerable<UserDemanda> demandados = db.userdemanda.Where(d => d.DemandaId == demanda.DemandaId && demanda.Delete == false).ToList();
                    demandados_list=demandados;
                    DemandaViewModel.Where(d => d.DemandaId == demanda.DemandaId).SingleOrDefault().Demandado = demandados_list;
                }
            }
            return View(DemandaViewModel);
        }
        public ActionResult DemandasReuniao()
        {
            return View();
        }
        public ActionResult Create(int id)
        {
            var ItemAssunto = db.itemassunto.Where(item => item.ItemAssuntoId == id).Include(a => a.Assunto).SingleOrDefault();
            ViewBag.ReuniaoId_ = db.itemassunto.Where(item=>item.ItemAssuntoId== id).Select(item=>item.ReuniaoId).SingleOrDefault();
            ViewBag.Quem = new SelectList(dbIdentity.Users, "UserName", "UserName", ItemAssunto.Responsavel);
            ViewBag.UserId_ = new SelectList(dbIdentity.Users, "UserName", "UserName", "--Selecione--");
            ViewBag.Demandado = new SelectList(dbIdentity.Users, "UserName", "UserName", "--Selecione--");

            ViewBag.Situacao = new SelectList(new[]
            {
                new {Valor = "Aberto", Texto = "Aberto"},
                new {Valor = "Encerrado", Texto = "Encerrado"},
                new {Valor = "Suspenso", Texto = "Suspenso"}
            }, "Valor", "Texto");
            
            ViewBag.Assunto = ItemAssunto.Assunto.DescricaoAs;
            ViewBag.Item = ItemAssunto.DescricaoItem;
            ViewBag.AssuntoId = new SelectList
            (
                db.Assunto.Where(i => i.AssuntoId == ItemAssunto.AssuntoId && i.Delete != true).ToList(),
                "AssuntoId",
                "DescricaoAs",ItemAssunto.AssuntoId
            );
            ViewBag.ItemAssuntoId = new SelectList
            (
                db.itemassunto.Where(i=>i.AssuntoId == ItemAssunto.AssuntoId && i.Delete != true).ToList(),
                "ItemAssuntoId",
                "DescricaoItem",id
            );
            return View();
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> CreateDemanda(CreateDemandaViewModel demandaview)
        {

            ApplicationDbContext _db = new ApplicationDbContext();
            string message = "";
            demandaview.InseridoEm = DateTime.UtcNow;
            
            var errors = new List<string>();
            string credencial = User.Identity.GetUserName();
            var userreuniao = db.userreuniao.Where(t => t.ReuniaoId == demandaview.ReuniaoId && t.UserName == credencial).SingleOrDefault();
            var success = "0";
            demandaview.Delete = false;
            demandaview.QuemInseriu = credencial;
            try
            {
                if (ModelState.IsValid && userreuniao.Perfil == "Moderador")
                {
                    Demanda DemandaDomain = Mapper.Map<Demanda>(demandaview);
                    db.Demanda.Add(DemandaDomain);

                    foreach (var item in demandaview.Demandado)
                    {
                        System.Diagnostics.Debug.WriteLine(item);
                        UserDemanda usrs = new UserDemanda()
                        {
                            UserId = item,
                            DemandaId = DemandaDomain.DemandaId,
                            InseridoEm = DateTime.Now,
                            ReuniaoId = DemandaDomain.ReuniaoId
                        };
                        db.userdemanda.Add(usrs);
                    }
                    db.SaveChanges();
                    message = "O Demanda foi atribuida com sucesso conforme plano de ação 6w2h";
                    //logCFG.Log("Reunião:" + db.reuniao.Find(demandaview.ReuniaoId).Nome + "-ID:" + demandaview.ReuniaoId + "|Por:" + credencial + " - Demanda cadastrada:" + db.itemassunto.Find(demandaview.ItemAssuntoId).DescricaoItem + "- Assunto:" + db.Assunto.Find(db.itemassunto.Find(demandaview.ItemAssuntoId).AssuntoId).DescricaoAs + "- Item: " + db.reuniao.Find(demandaview.ReuniaoId).Nome);
                    //var link = "http://localhost:8082/Reuniao/Ata/" + demandaview.ReuniaoId + "";
                    //var texto = "<p>Uma demanda foi atribuida para voce, sera responsavel do registrar o progresso da mesma e o que foi realizado até o momento:&nbsp;" + demandaview.Oque + " que pertence ao item:&nbsp;" + db.itemassunto.Find(demandaview.ItemAssuntoId).DescricaoItem + "e do assunto" + db.Assunto.Find(db.itemassunto.Find(demandaview.ItemAssuntoId).AssuntoId).DescricaoAs + ", para acessar a ATA da&nbsp;<b>Reunião:&nbsp;" + db.reuniao.Find(demandaview.ReuniaoId).Nome + "</b><a href='" + link + "'>&nbsp;Click aqui</p>";
                    //var idresp = _db.Users.Where(u => u.UserName == demandaview.Quem).Select(u => u.Id).SingleOrDefault();
                    //var emailresp = _db.Users.Find(idresp).Email;
                    //EmailService emailService = new EmailService();
                    //await emailService.SendEmailAsync(emailresp, texto, "SGR - Nova responsabilidade");
                    success = "true";
                    return Json(new { success = success, message = message, ReuniaoId = demandaview.ReuniaoId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    success = "0";
                    if (userreuniao.Perfil != "Moderador")
                    {
                        errors.Add("Você não possui privilegio para eecutar essa ação.");
                    }
                    foreach (var modelStateVal in ViewData.ModelState.Values)
                    {

                        errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                success = "0";
                errors.Add("Ocorreu o erro:" + ex);
            }
            if (success=="0"){
                return Json(new { success = false, message = errors, ReuniaoId = demandaview.ReuniaoId }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = success, message = message, ReuniaoId = demandaview.ReuniaoId }, JsonRequestBehavior.AllowGet);
        }
            //Editar somente demanda
        public ActionResult EditAcoes(int id)
        {
            Demanda dem = db.Demanda.Where(d => d.DemandaId == id).SingleOrDefault();
            EditDemandaViewModel DemandaViewModel = Mapper.Map<EditDemandaViewModel>(dem);
            List<UserDemanda> Demandados= db.userdemanda.Where(u => u.DemandaId == id).ToList();
            ViewBag.Demandado = db.userdemanda.Where(u => u.DemandaId == id).Select(d=>d.UserId).ToList();
            ViewBag.AssuntoId = new SelectList
            (
                db.Assunto.ToList(),
                "AssuntoId",
                "DescricaoAs", db.Demanda.Where(item => item.DemandaId == id).Include(item => item.itemassunto.Assunto.AssuntoId).Select(item=> item.itemassunto.Assunto.AssuntoId).SingleOrDefault()
            );
            ViewBag.ItemAssuntoId = new SelectList
            (
                db.itemassunto.ToList(),
                "ItemAssuntoId",
                "DescricaoItem", db.Demanda.Where(item => item.DemandaId == id).Include(item => item.itemassunto.ItemAssuntoId).Select(item => item.itemassunto.ItemAssuntoId).SingleOrDefault()
            );
            ViewBag.Situacao = new SelectList(new[]
            {
                new {Valor = "Aberto", Texto = "Aberto"},
                new {Valor = "Encerrado", Texto = "Encerrado"},
                new {Valor = "Suspenso", Texto = "Suspenso"}
            }, "Valor", "Texto", dem.Situacao);
            if (db.AcaoDemanda.Where(a => a.DemandaId == dem.DemandaId).Count() > 0)
            {
                ViewBag.AcoesCount = 1;
            }
            return View(DemandaViewModel);
        }
        [HttpPost]
        public JsonResult Edit(DemandaViewModel demandaview)
        {
            string message = "";
            var errors = new List<string>();
            if (ModelState.IsValid)
            {
                var DemandaDomain = Mapper.Map<DemandaViewModel, Demanda>(demandaview);
                db.Demanda.Attach(DemandaDomain);
                db.Entry(DemandaDomain).State = EntityState.Modified;
                db.SaveChanges();
                message = "sucesso";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
               
                foreach (var modelStateVal in ViewData.ModelState.Values)
                {
                    errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                }
                return Json(new { success = false, message = errors });
            }
            return Json(new { success = false, message = errors }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SelecionaDemandado(string id)
        {
            var UserImg = dbIdentity.Users.Where(s => s.UserName == id).SingleOrDefault();
            if (UserImg.ImagePath== null| UserImg.ImagePath.IsEmpty())
            {
                return Json(new { imagem = "/Images/Avatar/demandado.png" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { imagem = UserImg.ImagePath }, JsonRequestBehavior.AllowGet);
            }
     
        }

        public JsonResult GetDemandasPorUtem(int Id, string sidx, string sort, int page, int rows)
        {

            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var ReuniaoList = (from u in db.Demanda.Where(usr => usr.ItemAssuntoId == Id && usr.Delete!=true && usr.itemassunto.Delete != true).AsEnumerable()
                               join r in db.AcaoDemanda on u.DemandaId equals r.DemandaId into acaodemanda
                               from reu in acaodemanda.DefaultIfEmpty()
                               select new
                               {
                                   u.DemandaId,
                                   u.Situacao,
                                   u.Oque,
                                   u.Como,
                                   u.Porque,
                                   u.Onde, 
                                   u.Quando,
                                   //u.Demandado,
                                   UltimaAcao = "Teste"
                               });

            int totalRecords = ReuniaoList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

            if (sort.ToUpper() == "DESC")
            {
                ReuniaoList = ReuniaoList.OrderByDescending(t => t.DemandaId);
                ReuniaoList = ReuniaoList.Skip(pageIndex * pageSize).Take(pageSize);
            }

            else
            {
                ReuniaoList = ReuniaoList.OrderBy(t => t.DemandaId);
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

        public JsonResult GetDemanda(int ID, string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            
            var DemandaList = (from d in db.Demanda.Where(d=>d.ReuniaoId ==ID).AsEnumerable()
                               join t in db.itemassunto on d.ItemAssuntoId equals t.ItemAssuntoId
                               join a in db.Assunto on t.AssuntoId equals a.AssuntoId
                               select new
                               {
                                   d.DemandaId,
                                   d.Situacao,
                                   d.ReuniaoId,
                                   Assunto = a.DescricaoAs,
                                   Item= t.DescricaoItem,
                                   d.Oque,
                                   d.Como,
                                   d.Porque,
                                   d.Quanto,
                                   d.Quem,
                                   Demandado =   demandadoreu.RetornaDemandadosD(d.DemandaId),
                                   d.Onde,
                                   d.Quando, 
                                   Acao ="N/A" ,
                                   ProgressoAtual ="25%",
                               });

            int totalRecords = DemandaList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);           

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = DemandaList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PesquisaAutocompleteResponsavel(string term, int id)
        {
            var lista_userreuniao = db.userreuniao.Where(u => u.ReuniaoId == id);
            var autotexto = from d in dbIdentity.Users.AsEnumerable()
                            join responsavel in lista_userreuniao on d.UserName equals responsavel.UserName 
                select new
                {
                    d.Id,
                    d.UserName,
                    d.Email,
                    Img = d.ImagePath ?? "/Images/Avatar/demandado.jpg",
                    Setor = "Setor:" + d.Setor.Nome ?? "N/A"
                };
            if (term != null)
            {
                autotexto = from d in dbIdentity.Users.AsEnumerable().Where(d => d.UserName.Contains(term) || d.Setor.Nome.Contains(term))
                    join responsavel in lista_userreuniao on d.UserName equals responsavel.UserName
                    select new
                    {
                        d.Id,
                        d.UserName,
                        d.Email,
                        Img = d.ImagePath ?? "/Images/Avatar/demandado.jpg",
                        Setor = "Setor:" + d.Setor.Nome ?? "N/A"
                    };

            }
            return Json(new { autotexto }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult PesquisaAutocompleteUser(string term)
        {
            
            var autotexto = from d in dbIdentity.Users
                            select new
                            {
                                d.Id,
                                d.UserName,
                                d.Email,
                                Img = d.ImagePath ?? "/Images/Avatar/demandado.jpg",
                                Setor = "Setor:"+d.Setor.Nome ?? "N/A"
                            };
            if (term != null)
            {
                autotexto = from d in dbIdentity.Users.Where(d => d.UserName.Contains(term) || d.Setor.Nome.Contains(term))
                            select new
                        {
                            d.Id,
                            d.UserName,
                            d.Email,
                            Img = d.ImagePath ?? "/Images/Avatar/demandado.jpg",
                            Setor = "Setor:" + d.Setor.Nome ?? "N/A"
                        };

            }
            return Json(new { autotexto }, JsonRequestBehavior.AllowGet);
        }

    }
}