using AutoMapper;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities.SGR;
using IntraNet.Mod.SGR.Models.ViewModel;
using IntraNet.Security.ContextIdentity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IntraNet.Domain.Entities;
using IntraNet.Security.Models;
using Hangfire;
using Microsoft.Ajax.Utilities;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace IntraNet.Mod.SGR.Controllers
{
    [Authorize]
    public class ReuniaoController : Controller
    {
        
        private ApplicationDbContext dbIdentity = new ApplicationDbContext();
        private ContextSGR db = new ContextSGR();
        // GET: Reuniao
        public ActionResult Index()
        {
            return View();
        }
        // GET: Reuniaos/Create
        public ActionResult Create()
        {

            //ViewBag.UserId = new SelectList(dbIdentity.Users, "UserName","UserName");
            ViewBag.SetorId = new SelectList(dbIdentity.Setor, "SetorId", "Nome");
            ViewBag.Acesso = new SelectList(new[]
            {
                new {Value = "Participante", Text = "Participante"},
                new {Value = "Moderador", Text = "Moderador"}
            }, "Value", "Text","--Selecione--");
            return View();
        }
        public ActionResult CreateReuniao()
        {

            ViewBag.UserId = new SelectList(dbIdentity.Users, "UserName","UserName");
            ViewBag.SetorId = new SelectList(dbIdentity.Setor, "SetorId", "Nome");

            ViewBag.Acesso = new SelectList(new[]
            {
                new {Value = "Participante", Text = "Participante"},
                new {Value = "Moderador", Text = "Moderador"}
            }, "Value", "Text", "--Selecione--");
            return View();
        }
        [HttpGet]
        public ActionResult Confirma()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Moderador(int ReuniaoId)
        {
            return View();
        }

        public ActionResult Edit(int ?id)
        {
            if (id!=0 && id>0 && id!=null)
            {
                ViewBag.Acesso = new SelectList(new[]
                {
                    new {Value = "Participante", Text = "Participante"},
                    new {Value = "Moderador", Text = "Moderador"}
                }, "Value", "Text", "--Selecione--");
                Reuniao reuniao = db.reuniao.Where(r => r.ReuniaoId == id).SingleOrDefault();
                ViewBag.SetorId = new SelectList(dbIdentity.Setor, "SetorId", "Nome",reuniao.SetorId);
                ViewBag.ListaParticipantes = new SelectList(db.userreuniao.Where((d=>d.ReuniaoId ==id && d.Perfil =="Moderador")), "UserName", "UserName");
                ViewBag.ListaModerador = new SelectList(db.userreuniao.Where((d => d.ReuniaoId == id && d.Perfil == "Participante")), "UserName", "UserName");
                string hora = GestaoController.PegaHoraAtual();
                System.Diagnostics.Debug.Write(hora);
               
                EditReuniaoViewModel ReuniaoViewModel = Mapper.Map<EditReuniaoViewModel>(reuniao);
                ViewBag.DemandasAbertas = 0;
                ViewBag.DemandasEncerrado = 0;
                ViewBag.DemandasSuspenso = 0;
                ViewBag.itens = 0;
                ViewBag.Assuntos = 0;
                ViewBag.DemandasTotais = 0;

                if (db.Assunto.Where(a => a.Delete != true && a.ReuniaoId == id).Count() > 0)
                {
                    ViewBag.Assuntos = db.Assunto.Where(a => a.Delete != true && a.ReuniaoId == id).Count();
                    if (db.itemassunto.Where(i => i.Delete != true && i.ReuniaoId == id).Count() > 0)
                    {
                        ViewBag.Itens = db.itemassunto.Where(i => i.Delete != true && i.ReuniaoId == id).Count();
                        if (db.Demanda.Where(d=>d.Delete != true && d.ReuniaoId ==id ).Count()>0)
                        {
                            ViewBag.DemandasTotais = db.Demanda.Where(d => d.Delete != true && d.ReuniaoId == id).Count();
                            if (db.Demanda.Where(d => d.Delete != true && d.Situacao == "Aberto" && d.ReuniaoId == reuniao.ReuniaoId).Count() > 0)
                            {
                                ViewBag.DemandasAbertas = db.Demanda.Where(d => d.Delete != true && d.Situacao == "Aberto" && d.ReuniaoId == reuniao.ReuniaoId).Count();
                            }
                            if (db.Demanda.Where(d => d.Delete != true && d.Situacao == "Encerrado" && d.ReuniaoId == reuniao.ReuniaoId).Count() > 0)
                            {
                                ViewBag.DemandasEncerrado = db.Demanda.Where(d => d.Delete != true && d.Situacao == "Encerrado" && d.ReuniaoId == reuniao.ReuniaoId).Count();
                            }
                            if (db.Demanda.Where(d => d.Delete != true && d.Situacao == "Suspenso" && d.ReuniaoId == reuniao.ReuniaoId).Count() > 0)
                            {
                                ViewBag.DemandasSuspenso = db.Demanda.Where(d => d.Delete != true && d.Situacao == "Suspenso" && d.ReuniaoId == reuniao.ReuniaoId).Count();
                            }
                        }
                    }
                }
                return View(ReuniaoViewModel);
            }
            else
            {
                ViewBag.Mensagem = "Selecione a para editar";
                return View();
            }
        }
        [HttpGet]
        public ActionResult Ata(int ID)
        {
            ContextSGR db = new ContextSGR();
            string iduser = User.Identity.GetUserId();
            ViewBag.AssuntosQtd = "0";
            ViewBag.ItensQtd = "0";
            ViewBag.DemandasQtd = "0";
            var acesso = db.userreuniao.Where(ac => ac.UserId == iduser && ac.ReuniaoId == ID).FirstOrDefault();
            try
            {
                if (acesso.Perfil == "Moderador")
                {

                    Reuniao reuniao = db.reuniao.Where(i => i.ReuniaoId == ID).First();
                    
                    string tema =dbIdentity.Setor.Where(set=>set.SetorId==reuniao.SetorId).Select(set=>set.Nome).SingleOrDefault();
                    ViewBag.ReuTema = tema;
                    ViewBag.ReuNome = reuniao.Nome;
                    ViewBag.ReuniaoId = reuniao.ReuniaoId;
                    string hora = GestaoController.PegaHoraAtual();
                    ViewBag.ReuHora = hora;
                    System.Diagnostics.Debug.Write(hora);
                    var ReuniaoViewModel = Mapper.Map<Reuniao, ReuniaoViewModel>(reuniao);
                    if (db.Assunto.Where(r => r.ReuniaoId == ID && r.Delete!=true).Count() > 0)
                    {
                        ViewBag.AssuntosQtd = db.Assunto.Where(r => r.ReuniaoId == ID && r.Delete != true).Count();
                        if (db.itemassunto.Where(r => r.ReuniaoId == ID && r.Delete != true).Count() > 0)
                        {
                            ViewBag.ItensQtd = db.itemassunto.Where(r => r.ReuniaoId == ID && r.Delete != true && r.Delete != true).Count();
                            if (db.Demanda.Where(r => r.ReuniaoId == ID && r.Delete != true).Count() > 0)
                            {
                                ViewBag.DemandasQtd = db.Demanda.Where(r => r.ReuniaoId == ID && r.Delete != true).Count();
                            }
                        }
                    }
                    return View(ReuniaoViewModel);
                }
                else
                {
                    //se usuário for nivel inferior KKK
                    return RedirectToAction("Ata", new { ID = ID });
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //GRIDS FUNCTION
        public JsonResult GetReuniao(string sidx, string sort, int page, int rows)
        {

            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var ReuniaoList = (from r in db.reuniao
                             join t in dbIdentity.Setor on r.SetorId equals t.SetorId
                             //from e in db.Empresa.Where(e => e.EmpresaId == r.EmpresaId).Select(n => n.RazaoSocial)
                             select new
                             {
                                 r.ReuniaoId,
                                 r.Nome,
                                 r.Observac,
                                 Tema = t.Nome,
                                 TotalDemandas = (db.Demanda.Where(d => d.ReuniaoId == r.ReuniaoId).Count()),
                                 Ativo = r.Delete
                             });

            int totalRecords = ReuniaoList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

            if (sort.ToUpper() == "DESC")
            {
                ReuniaoList = ReuniaoList.OrderByDescending(t => t.ReuniaoId);
                ReuniaoList = ReuniaoList.Skip(pageIndex * pageSize).Take(pageSize);
            }

            else
            {
                ReuniaoList = ReuniaoList.OrderBy(t => t.ReuniaoId);
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

        public string SituacaoReuniao(int id)
        {
            List<Assunto> Assunto = db.Assunto.Where(a => a.ReuniaoId == id).ToList();
            string situacao = "Incompleta";
            // var pendentes = Demandas.Where(d => d.Situacao == "Aberto").SingleOrDefault();
            if (Assunto.Count() > 0 && Assunto != null)
            {
                if (Assunto.Where(r => r.Situacao == "Aberto").Count() > 0)
                {
                    situacao = "Pendente";
                }
                else
                {
                    situacao = "Concluido";
                }

            }
            return situacao;
        }

        public string RetornaParticipantes(int id, string perfil)
        {
            string participantes = "somente com acesso moderador";
            var userreuniao_id = db.userreuniao.Where(i => i.ReuniaoId == id).Select(i=>i.UserName).ToList();

            if (perfil == "Moderador")
            {
                participantes = "";
                foreach (var item in userreuniao_id)
                {
                    participantes = participantes + item + ", ";
                }
            }

            return participantes;
        }
        public JsonResult GetReuniaoUser(string sidx, string sort, int page, int rows)
        {
            var username = User.Identity.GetUserName();
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var credencial = db.userreuniao.Where(u => u.UserName == username).Select(u => u.Perfil);
            var ReuniaoList = db.userreuniao.Where(usr => usr.UserName == username && usr.reuniao.Delete!=true).Include(usr=>usr.reuniao).AsEnumerable().Select(a => new
            {
                Situacao = SituacaoReuniao(a.ReuniaoId),
                a.ReuniaoId,
                a.reuniao.Nome,
                Acesso = a.Perfil,
                a.reuniao.Observac,
                Tema = dbIdentity.Setor.Where(set=>set.SetorId==a.reuniao.SetorId).Select(set=>set.Nome).SingleOrDefault(),
                Participantes = RetornaParticipantes(a.ReuniaoId, a.Perfil),
                TotalDemandas = ""+ db.userdemanda.DefaultIfEmpty().Where(d => d.ReuniaoId == a.ReuniaoId && d.Delete != true && d.UserId == username && d.demanda.Delete != true && d.demanda.itemassunto.Delete != true).Count() + "/"+ db.Demanda.DefaultIfEmpty().Where(dem => dem.ReuniaoId == a.ReuniaoId && dem.Delete != true && dem.itemassunto.Delete !=true && dem.itemassunto.Assunto.Delete != true).Count()+""
            });
            

            int totalRecords = ReuniaoList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

            if (sort.ToUpper() == "DESC")
            {
                ReuniaoList = ReuniaoList.OrderByDescending(t => t.ReuniaoId);
                ReuniaoList = ReuniaoList.Skip(pageIndex * pageSize).Take(pageSize);
            }

            else
            {
                ReuniaoList = ReuniaoList.OrderBy(t => t.ReuniaoId);
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

        public JsonResult Delete(int Id)
        {
            string msg;
            var errors = new List<string>();

            string credencial = User.Identity.GetUserName();
            Reuniao Reuniao = db.reuniao.Where(a => a.ReuniaoId == Id).Single();
            var userreuniao = db.userreuniao.Where(t => t.ReuniaoId == Reuniao.ReuniaoId && t.UserName == credencial).SingleOrDefault();
            try
            {

                //compara o perfil do usuário
                if (userreuniao.Perfil == "Moderador")
                {
                    Reuniao.Delete = true;
                    if (ModelState.IsValid)
                    {
                        Domain.Entities.SGR.Reuniao ReuniaoDomain = Mapper.Map<Reuniao>(Reuniao);
                        db.Entry(Reuniao).State = EntityState.Detached;
                        db.Entry(Reuniao).State = EntityState.Modified;
                        db.SaveChanges();
                        msg = "A reunião foi removido com sucesso";
                        //AssuntoOriginal = db.Assunto.Where(a => a.AssuntoId == Assunto.AssuntoId).FirstOrDefault();
                        //logCFG.Log("Reunião:" + db.reuniao.Find(Assunto.ReuniaoId).Nome + "-ID:" + AssuntoDomain.ReuniaoId + " - Exclusao de assunto |Por:" + credencial);
                        return Json(new { success = true, message = msg }, JsonRequestBehavior.AllowGet);
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
                //logCFG.Log("Reunião:" + db.reuniao.Find(Assunto.ReuniaoId).Nome + "-ID:" + Assunto.ReuniaoId + "| Por:" + credencial + " - Ocorreu uma tentativa de Excluir o assunto  -|Erro:" + errors);
                return Json(new { success = false, message = errors});
            }
            //Assunto = db.Assunto.Where(a => a.AssuntoId == Assunto.AssuntoId).FirstOrDefault();
            //logCFG.Log("Reunião:" + db.reuniao.Find(Assunto.ReuniaoId).Nome + "-ID:" + Assunto.ReuniaoId + "| Por:" + credencial + "  - Ocorreu uma tentativa de Excluir o assunto -|Erro:" + errors);
            return Json(new { success = false, message = errors, ReuniaoId = Reuniao.ReuniaoId });
        }

        [HttpPost]
        public async Task<JsonResult> Create(CreateReuniaoViewModel reuniaoview)
        {
            //CreateReuniaoViewModel reuniaoview = new CreateReuniaoViewModel();
            string message = "";
            var errors = new List<string>();
            //reuniaoview.Nome = nome;
            //reuniaoview.SetorId = setorid;
            reuniaoview.InseridoEm = DateTime.Now;
            reuniaoview.Delete = false;
            //reuniaoview.ListaParticipantes = ListaParticipantes;
            try
            {
                if(ModelState.IsValid && reuniaoview.ListaModerador!= null)
                {                    
                    //string[] ListaModeradorSerial = new JavaScriptSerializer().Deserialize<string[]>(reuniaoview.ListaModerador.ToString());
                    var ReuniaoDomain = Mapper.Map<CreateReuniaoViewModel, Reuniao>(reuniaoview);
                    var user = new ApplicationUser();
                    db.reuniao.Add(ReuniaoDomain);
                    foreach (var item in reuniaoview.ListaModerador)
                    {
                        user = dbIdentity.Users.Where(n => n.UserName == item).Single();
                        var users = new UserReuniao()
                        {
                            UserId = user.Id,
                            UserName = item.ToString(),
                            ReuniaoId = ReuniaoDomain.ReuniaoId,
                            Perfil = "Moderador",
                            Delete = false,
                            InseridoEm = DateTime.Now
                        };
                        db.userreuniao.Add(users);
                        //user = dbIdentity.Users.Where(n => n.UserName == item).Single();
                        //logCFG.Log("Reunião:" + db.reuniao.Find(reuniaoview.ReuniaoId).Nome + "-ID:" + reuniaoview.ReuniaoId + "|Por:" + credencial + " - Demanda cadastrada:" + db.itemassunto.Find(demandaview.ItemAssuntoId).DescricaoItem + "- Assunto:" + db.Assunto.Find(db.itemassunto.Find(demandaview.ItemAssuntoId).AssuntoId).DescricaoAs + "- Item: " + db.reuniao.Find(demandaview.ReuniaoId).Nome);
                        //var link = "http://localhost:8082/Reuniao/Ata/" + reuniaoview.ReuniaoId + "";
                        //var texto = "<p>Voce está participando da reuniao:" + reuniaoview.Nome + ", todo progresso devera ser incluido no sistema:&nbsp;Seu acesso é de: Participante" + "</b><a href='" + link + "'>&nbsp;Click aqui</p>";
                        //var emailuser = dbIdentity.Users.Where(u => u.UserName == item).Select(u => u.Email).SingleOrDefault();
                        //EmailService emailService = new EmailService();
                        //BackgroundJob.Schedule<EmailService>(x => x.SendEmailAsync(emailuser, texto, "SGR - Nova reuniao"), TimeSpan.FromMilliseconds(1));

                    }
                    if(reuniaoview.ListaParticipantes!=null)
                    {                      
                        foreach (var item in reuniaoview.ListaParticipantes)
                        {                            
                            var users = new UserReuniao()
                            {
                                UserId = user.Id,
                                UserName = item,
                                ReuniaoId = ReuniaoDomain.ReuniaoId,
                                Perfil = "Participante",
                                Delete = false,
                                InseridoEm = DateTime.Now
                            };
                            db.userreuniao.Add(users);
                            user = dbIdentity.Users.Where(n => n.UserName == item).Single();
                            //logCFG.Log("Reunião:" + db.reuniao.Find(reuniaoview.ReuniaoId).Nome + "-ID:" + reuniaoview.ReuniaoId + "|Por:" + credencial + " - Demanda cadastrada:" + db.itemassunto.Find(demandaview.ItemAssuntoId).DescricaoItem + "- Assunto:" + db.Assunto.Find(db.itemassunto.Find(demandaview.ItemAssuntoId).AssuntoId).DescricaoAs + "- Item: " + db.reuniao.Find(demandaview.ReuniaoId).Nome);
                            //var link = "http://localhost:8082/Reuniao/Ata/" + reuniaoview.ReuniaoId + "";
                            //var texto = "<p>Voce está participando da reuniao:" + reuniaoview.Nome + ", todo progresso devera ser incluido no sistema:&nbsp;Seu acesso é de: Participante" + "</b><a href='" + link + "'>&nbsp;Click aqui</p>";
                            //var emailuser = dbIdentity.Users.Where(u => u.UserName == item).Select(u => u.Email).SingleOrDefault();
                            //EmailService emailService = new EmailService();
                            //BackgroundJob.Schedule<EmailService>(x => x.SendEmailAsync(emailuser, texto, "SGR - Nova reuniao"), TimeSpan.FromMilliseconds(1));
                            
                        }
                    }
                    message = "Cadastrada com sucesso";
                    db.SaveChanges();                    
                    return Json(new { success = 1, message = message, reuniaoid = ReuniaoDomain.ReuniaoId }, JsonRequestBehavior.AllowGet);
                   
                }
                else
                {
                    
                    if (reuniaoview.ListaModerador.Count() < 0 || reuniaoview.ListaModerador == null) { errors.Add("É obrigatório definir pelo menos um usuário com acesso moderador."); }
                    foreach (var modelStateVal in ViewData.ModelState.Values)
                    {
                        errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                    }
                    return Json(new { success = 0, message = errors }, JsonRequestBehavior.AllowGet);
                }
            }catch(Exception e)
            {
                if (reuniaoview.ListaModerador == null) {
                    errors.Add("É obrigatório definir pelo menos um usuário com acesso moderador.");
                }
                if (errors.Count() <1)
                {
                    errors.Add("Verifique os campos -"+e.Message);
                }
                return Json(new { success = 0, message = errors }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<JsonResult> CreateSerializado(string nome, int setorid, string[] ListaParticipantes, string[] ListaModerador)
        {
            CreateReuniaoViewModel reuniaoview = new CreateReuniaoViewModel();
            string message = "";
            reuniaoview.Nome = nome;
            reuniaoview.SetorId = setorid;
            reuniaoview.InseridoEm = DateTime.Now;
            reuniaoview.Delete = false;
            reuniaoview.ListaParticipantes = ListaParticipantes;

            if (ModelState.IsValid && ListaModerador.Count() > 0 && ListaModerador != null && ListaModerador.First() != "" && ListaModerador.First() != " ")
            {
                try
                {
                    string[] ListaModeradorSerial = new JavaScriptSerializer().ConvertToType<string[]>(ListaParticipantes);
                    var ReuniaoDomain = Mapper.Map<CreateReuniaoViewModel, Reuniao>(reuniaoview);
                    var user = new ApplicationUser();
                    db.reuniao.Add(ReuniaoDomain);
                    foreach (var item in ListaModeradorSerial)
                    {
                        user = dbIdentity.Users.Where(n => n.UserName == item.ToString()).Single();

                        var users = new UserReuniao()
                        {
                            UserId = user.Id,
                            UserName = item.ToString(),
                            ReuniaoId = ReuniaoDomain.ReuniaoId,
                            Perfil = "Moderador",
                            Delete = false,
                            InseridoEm = DateTime.Now
                        };
                        db.userreuniao.Add(users);
                        //user = dbIdentity.Users.Where(n => n.UserName == item).Single();
                        //logCFG.Log("Reunião:" + db.reuniao.Find(reuniaoview.ReuniaoId).Nome + "-ID:" + reuniaoview.ReuniaoId + "|Por:" + credencial + " - Demanda cadastrada:" + db.itemassunto.Find(demandaview.ItemAssuntoId).DescricaoItem + "- Assunto:" + db.Assunto.Find(db.itemassunto.Find(demandaview.ItemAssuntoId).AssuntoId).DescricaoAs + "- Item: " + db.reuniao.Find(demandaview.ReuniaoId).Nome);
                        //var link = "http://localhost:8082/Reuniao/Ata/" + reuniaoview.ReuniaoId + "";
                        //var texto = "<p>Voce está participando da reuniao:" + reuniaoview.Nome + ", todo progresso devera ser incluido no sistema:&nbsp;Seu acesso é de: Participante" + "</b><a href='" + link + "'>&nbsp;Click aqui</p>";
                        //var emailuser = dbIdentity.Users.Where(u => u.UserName == item).Select(u => u.Email).SingleOrDefault();
                        //EmailService emailService = new EmailService();
                        //BackgroundJob.Schedule<EmailService>(x => x.SendEmailAsync(emailuser, texto, "SGR - Nova reuniao"), TimeSpan.FromMilliseconds(1));

                    }

                    message = "Cadastrada com sucesso";
                    db.SaveChanges();

                    if (ListaParticipantes != null && ListaParticipantes.First() != "" && ListaParticipantes.First() != " ")
                    {
                        string[] ListaParticipantesSerial = new JavaScriptSerializer().ConvertToType<string[]>(ListaParticipantes);
                        foreach (var item in ListaParticipantesSerial)
                        {
                            user = dbIdentity.Users.Where(n => n.UserName == item).Single();

                            var users = new UserReuniao()
                            {
                                UserId = user.Id,
                                UserName = item,
                                ReuniaoId = ReuniaoDomain.ReuniaoId,
                                Perfil = "Participante",
                                Delete = false,
                                InseridoEm = DateTime.Now
                            };

                            db.userreuniao.Add(users);
                            user = dbIdentity.Users.Where(n => n.UserName == item).Single();
                            //logCFG.Log("Reunião:" + db.reuniao.Find(reuniaoview.ReuniaoId).Nome + "-ID:" + reuniaoview.ReuniaoId + "|Por:" + credencial + " - Demanda cadastrada:" + db.itemassunto.Find(demandaview.ItemAssuntoId).DescricaoItem + "- Assunto:" + db.Assunto.Find(db.itemassunto.Find(demandaview.ItemAssuntoId).AssuntoId).DescricaoAs + "- Item: " + db.reuniao.Find(demandaview.ReuniaoId).Nome);
                            var link = "http://localhost:8082/Reuniao/Ata/" + reuniaoview.ReuniaoId + "";
                            var texto = "<p>Voce está participando da reuniao:" + reuniaoview.Nome + ", todo progresso devera ser incluido no sistema:&nbsp;Seu acesso é de: Participante" + "</b><a href='" + link + "'>&nbsp;Click aqui</p>";
                            var emailuser = dbIdentity.Users.Where(u => u.UserName == item).Select(u => u.Email).SingleOrDefault();
                            //EmailService emailService = new EmailService();
                            //BackgroundJob.Schedule<EmailService>(x => x.SendEmailAsync(emailuser, texto, "SGR - Nova reuniao"), TimeSpan.FromMilliseconds(1));
                            db.SaveChanges();
                        }
                    }
                    return Json(new { success = 1, message = message }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { success = 0, message = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var errors = new List<string>();
                if (ListaModerador.Count() < 0 || ListaModerador == null) { errors.Add("É obrigatório definir pelo menos um usuário com acesso moderador."); }
                foreach (var modelStateVal in ViewData.ModelState.Values)
                {
                    errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                }
                return Json(new { success = 0, message = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Edit(EditReuniaoViewModel reuniaoview)
        {
            
            string message = "";
            
            reuniaoview.InseridoEm = DateTime.Now;
            reuniaoview.Delete = false;
            if (ModelState.IsValid && reuniaoview.ListaModerador!=null)
            {
                try
                {
                    //string[] ListaModeradorSerial = new JavaScriptSerializer().ConvertToType<string[]>(ListaParticipantes);
                    var ReuniaoDomain = Mapper.Map<EditReuniaoViewModel, Reuniao>(reuniaoview);
                    var user = new ApplicationUser();
                    //db.reuniao.Add(ReuniaoDomain);
                    db.Entry(ReuniaoDomain).State = EntityState.Detached;
                    db.Entry(ReuniaoDomain).State = EntityState.Modified;
                    db.SaveChanges();
                    //remove usuários para adicionar novamente
                    IEnumerable<UserReuniao> users_reuniao = db.userreuniao.Where(u => u.ReuniaoId == reuniaoview.ReuniaoId).ToList();
                    db.userreuniao.RemoveRange(users_reuniao);
                    foreach (var item in reuniaoview.ListaModerador)
                    {
                        user = dbIdentity.Users.Where(n => n.UserName == item.ToString()).Single();

                        var users = new UserReuniao()
                        {
                            UserId = user.Id,
                            UserName = item.ToString(),
                            ReuniaoId = ReuniaoDomain.ReuniaoId,
                            Perfil = "Moderador",
                            Delete = false,
                            InseridoEm = DateTime.Now
                        };
                        db.userreuniao.Add(users);
                        //user = dbIdentity.Users.Where(n => n.UserName == item).Single();
                        //logCFG.Log("Reunião:" + db.reuniao.Find(reuniaoview.ReuniaoId).Nome + "-ID:" + reuniaoview.ReuniaoId + "|Por:" + credencial + " - Demanda cadastrada:" + db.itemassunto.Find(demandaview.ItemAssuntoId).DescricaoItem + "- Assunto:" + db.Assunto.Find(db.itemassunto.Find(demandaview.ItemAssuntoId).AssuntoId).DescricaoAs + "- Item: " + db.reuniao.Find(demandaview.ReuniaoId).Nome);
                        //var link = "http://localhost:8082/Reuniao/Ata/" + reuniaoview.ReuniaoId + "";
                        //var texto = "<p>Voce está participando da reuniao:" + reuniaoview.Nome + ", todo progresso devera ser incluido no sistema:&nbsp;Seu acesso é de: Participante" + "</b><a href='" + link + "'>&nbsp;Click aqui</p>";
                        //var emailuser = dbIdentity.Users.Where(u => u.UserName == item).Select(u => u.Email).SingleOrDefault();
                        //EmailService emailService = new EmailService();
                        //BackgroundJob.Schedule<EmailService>(x => x.SendEmailAsync(emailuser, texto, "SGR - Nova reuniao"), TimeSpan.FromMilliseconds(1));

                    }

                    message = "Alterada com sucesso com sucesso";
                    db.SaveChanges();

                    if (reuniaoview.ListaParticipantes != null)
                    {
                        //List<string> ListaParticipantesSerial = new JavaScriptSerializer().ConvertToType<List<string>>(ListaParticipantes);
                        foreach (var item in reuniaoview.ListaParticipantes)
                        {
                            user = dbIdentity.Users.Where(n => n.UserName == item).Single();

                            var users = new UserReuniao()
                            {
                                UserId = user.Id,
                                UserName = item,
                                ReuniaoId = ReuniaoDomain.ReuniaoId,
                                Perfil = "Participante",
                                Delete = false,
                                InseridoEm = DateTime.Now
                            };

                            db.userreuniao.Add(users);
                            user = dbIdentity.Users.Where(n => n.UserName == item).Single();
                            //logCFG.Log("Reunião:" + db.reuniao.Find(reuniaoview.ReuniaoId).Nome + "-ID:" + reuniaoview.ReuniaoId + "|Por:" + credencial + " - Demanda cadastrada:" + db.itemassunto.Find(demandaview.ItemAssuntoId).DescricaoItem + "- Assunto:" + db.Assunto.Find(db.itemassunto.Find(demandaview.ItemAssuntoId).AssuntoId).DescricaoAs + "- Item: " + db.reuniao.Find(demandaview.ReuniaoId).Nome);
                            var link = "http://localhost:8082/Reuniao/Ata/" + reuniaoview.ReuniaoId + "";
                            var texto = "<p>Voce está participando da reuniao:" + reuniaoview.Nome + ", todo progresso devera ser incluido no sistema:&nbsp;Seu acesso é de: Participante" + "</b><a href='" + link + "'>&nbsp;Click aqui</p>";
                            var emailuser = dbIdentity.Users.Where(u => u.UserName == item).Select(u => u.Email).SingleOrDefault();
                            //EmailService emailService = new EmailService();
                            //BackgroundJob.Schedule<EmailService>(x => x.SendEmailAsync(emailuser, texto, "SGR - Nova reuniao"), TimeSpan.FromMilliseconds(1));
                            db.SaveChanges();
                        }
                    }
                    return Json(new { success = 1, message = message }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { success = 0, message = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var errors = new List<string>();
                if (reuniaoview.ListaModerador.Count() < 0 || reuniaoview.ListaModerador == null) { errors.Add("É obrigatório definir pelo menos um usuário com acesso moderador."); }
                foreach (var modelStateVal in ViewData.ModelState.Values)
                {
                    errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                }
                return Json(new { success = 0, message = errors }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult ListaUsuarios()
        {
            List<string> d = new List<string>();
            foreach (var cat in dbIdentity.Users.ToList())
            {
                d.Add(cat.UserName);
            }
            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }
    }
}