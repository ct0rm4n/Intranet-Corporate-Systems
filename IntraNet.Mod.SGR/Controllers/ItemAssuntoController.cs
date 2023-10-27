using AutoMapper;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities;
using IntraNet.Domain.Entities.SGR;
using IntraNet.Mod.SGR.Models.ViewModel;
using IntraNet.Security.ContextIdentity;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IntraNet.Mod.SGR.Controllers
{
    public class ItemAssuntoController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        public LogController logCFG = new LogController();
        public ContextSGR db = new ContextSGR();
        // GET: ItemAssunto
        public ActionResult Index()
        {
            return View();
        }

        public string RetornaSolicitante(int id)
        {
            //retorna todos os usuários que solicitaram demandas
            string solicitantes = "";
            var u_solicitante = db.Demanda.Where(i => i.ItemAssuntoId == id).Select(d => d.Quem).Distinct().ToList();
            foreach (var item in u_solicitante)
            {
                solicitantes = solicitantes  + item + ", ";
            }
            return solicitantes;
        }
        public string RetornaDemandados(int id)
        {
            //retorna todos os usuários que solicitaram demandas
            string demandados = "";
            var ids_demandas = db.Demanda.Where(i => i.ItemAssuntoId == id).Select(d => d.DemandaId).ToList();
            var u_demandado = db.userdemanda.Where(x => ids_demandas.Contains(x.DemandaId)).Select(x=>x.UserId).Distinct().ToList();
            foreach (var item in u_demandado)
            {
                demandados = demandados + item + ", ";
            }

            return demandados;
        }

        public JsonResult GetAssuntosItensReuniao(int ID, string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var subGridData = (from i in db.Assunto.Where(i => i.ReuniaoId == ID && i.Delete == false).AsEnumerable()
                               join a in db.Assunto on i.AssuntoId equals a.AssuntoId
                               select new
                               {
                                   //i.ItemAssuntoId,
                                   Assunto = a.DescricaoAs,
                                   //i.DescricaoItem,
                                   i.AssuntoId,
                                   i.ReuniaoId,
                                   i.Situacao,
                                   //i.Responsavel,
                                   //i.InseridoEm,
                                   //i.Prioridade,
                                   //i.QuemInseriu,
                                   Quem = "INSIRA UM ITEM",
                                   Demandado = "",
                                   TotalDemandas = 0
                               });


            if (db.itemassunto.Where(i => i.ReuniaoId == ID && i.Delete == false && i.Assunto.Delete != true).Count()>0) {
                var subGridDataItem = (from i in db.itemassunto.Where(i => i.ReuniaoId == ID && i.Delete == false && i.Assunto.Delete != true).AsEnumerable()
                                   join a in db.Assunto on i.AssuntoId equals a.AssuntoId
                                   select new
                                   {
                                       i.ItemAssuntoId,
                                       Assunto = a.DescricaoAs,
                                       i.DescricaoItem,
                                       i.AssuntoId,
                                       i.ReuniaoId,
                                       i.Situacao,
                                       i.Responsavel,
                                       i.InseridoEm,
                                       i.Prioridade,
                                       i.QuemInseriu,
                                       Quem = (RetornaSolicitante(i.ItemAssuntoId)).ToString(),
                                       Demandado = (RetornaDemandados(i.ItemAssuntoId)).ToString(),
                                       TotalDemandas = (db.Demanda.Where(d => d.ItemAssuntoId == i.ItemAssuntoId).Count())
                                   });
                int totalRecordsI = subGridDataItem.Count();
                var totalPagesI = (int)Math.Ceiling((float)totalRecordsI / (float)rows);
                if (sort.ToUpper() == "DESC")
                {
                    subGridDataItem = subGridDataItem.OrderByDescending(t => t.ItemAssuntoId);
                    subGridDataItem = subGridDataItem.Skip(pageIndex * pageSize).Take(pageSize);
                }
                else
                {
                    subGridDataItem = subGridDataItem.OrderByDescending(t => t.ItemAssuntoId);
                    subGridDataItem = subGridDataItem.Skip(pageIndex * pageSize).Take(pageSize);
                }
                var jsonDataI = new
                {
                    total = totalPagesI,
                    page,
                    records = totalRecordsI,
                    rows = subGridDataItem
                };
                System.Diagnostics.Debug.WriteLine("Reuniao do tema:" + ID);
                return Json(jsonDataI, behavior: JsonRequestBehavior.AllowGet);
            }
            

            int totalRecords = subGridData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                subGridData = subGridData.OrderByDescending(t => t.AssuntoId);
                subGridData = subGridData.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                subGridData = subGridData.OrderByDescending(t => t.AssuntoId);
                subGridData = subGridData.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = subGridData
            };
            System.Diagnostics.Debug.WriteLine("Reuniao do tema:" + ID);
            return Json(jsonData, behavior: JsonRequestBehavior.AllowGet);
        }
        public JsonResult ItemAssuntoSubGrid(int ID, string sidx, string sort, int page, int rows)
        {
            
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var subGridData = (from i in db.itemassunto.Where(i => i.AssuntoId == ID && i.Delete == false).DefaultIfEmpty().AsEnumerable()
                join a in db.Assunto on i.AssuntoId equals a.AssuntoId
                select new
                {
                    i.ItemAssuntoId,
                    Assunto = a.DescricaoAs,
                    i.DescricaoItem,
                    i.AssuntoId,
                    i.ReuniaoId,
                    i.Situacao,
                    i.Responsavel,
                    i.InseridoEm,
                    i.Prioridade,
                    i.QuemInseriu,
                    Quem = (RetornaSolicitante(i.ItemAssuntoId)).ToString(),
                    Demandado = (RetornaDemandados(i.ItemAssuntoId)).ToString(),
                    TotalDemandas = (db.Demanda.Where(d => d.ItemAssuntoId == i.ItemAssuntoId).Count())
                });
            
            int totalRecords = subGridData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                subGridData = subGridData.OrderByDescending(t => t.ItemAssuntoId);
                subGridData = subGridData.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                subGridData = subGridData.OrderByDescending(t => t.ItemAssuntoId);
                subGridData = subGridData.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = subGridData
            };
            System.Diagnostics.Debug.WriteLine("Reuniao do tema:" + ID);
            return Json(jsonData, behavior: JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int Id)
        {
            string msg;
            var errors = new List<string>();
            string credencial = User.Identity.GetUserName();
            ItemAssunto ItemAssunto = db.itemassunto.Where(a => a.ItemAssuntoId == Id).Single();
            var userreuniao = db.userreuniao.Where(t => t.ReuniaoId == ItemAssunto.ReuniaoId && t.UserName == credencial).SingleOrDefault();
            try
            {

                ItemAssunto.Delete = true;
                if (ModelState.IsValid && userreuniao.Perfil == "Moderador")
                {
                    Domain.Entities.SGR.ItemAssunto AssuntoDomain = Mapper.Map<ItemAssunto>(ItemAssunto);
                    db.Entry(ItemAssunto).State = EntityState.Detached;
                    db.Entry(ItemAssunto).State = EntityState.Modified;
                    db.SaveChanges();
                    msg = "O item" + ItemAssunto.DescricaoItem + " do assunto " + db.Assunto.Find(ItemAssunto.AssuntoId).DescricaoAs + " foi removido com sucesso";
                    //AssuntoOriginal = db.Assunto.Where(a => a.AssuntoId == Assunto.AssuntoId).FirstOrDefault();
                    //logCFG.Log("Reunião:" + db.reuniao.Find(ItemAssunto.ReuniaoId).Nome + "-ID:" + AssuntoDomain.ReuniaoId + " - Exclusao O item" + ItemAssunto.DescricaoItem + " do assunto " + db.Assunto.Find(ItemAssunto.ReuniaoId).DescricaoAs + "  |Por:" + credencial);
                    return Json(new { success = true, message = msg, ReuniaoId = ItemAssunto.ReuniaoId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (userreuniao.Perfil != "Moderador") { errors.Add("Voce não possui acesso para realizar esta alteração"); }
                    foreach (var modelStateVal in ViewData.ModelState.Values)
                    {
                        errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add("Error occured:" + ex.Message);
                //logCFG.Log("Reunião:" + db.reuniao.Find(ItemAssunto.ReuniaoId).Nome + "-ID:" + ItemAssunto.ReuniaoId + " - Ocorreu uma tentativa de Excluir o assunto |Por:" + credencial + "-|Erro:" + errors);
                return Json(new { success = false, message = errors, ReuniaoId = ItemAssunto.ReuniaoId });
            }
            //ItemAssunto = db.itemassunto.Where(a => a.AssuntoId == ItemAssunto.AssuntoId).FirstOrDefault();
            //logCFG.Log("Reunião:" + db.reuniao.Find(ItemAssunto.ReuniaoId).Nome + "-ID:" + ItemAssunto.ReuniaoId + " - Ocorreu uma tentativa de Excluir o assunto |Por:" + credencial + "-|Erro:" + errors);
            return Json(new { success = false, message = errors, ReuniaoId = ItemAssunto.ReuniaoId });
        }
        [HttpPost]
        public async Task<JsonResult> Edit(ItemAssuntoViewModel model)
        {
            string msg;
            var errors = new List<string>();
            var ItemAssuntoOriginal = new ItemAssunto();
            ItemAssuntoOriginal = db.itemassunto.Where(a => a.ItemAssuntoId == model.ItemAssuntoId).FirstOrDefault();
            string credencial = User.Identity.GetUserName();
            var userreuniao = db.userreuniao.Where(t => t.ReuniaoId == model.ReuniaoId && t.UserName == credencial).SingleOrDefault();
            try
            {

                if (ModelState.IsValid && userreuniao.Perfil == "Moderador")
                {
                    model.Delete = false;
                    
                    ItemAssunto ItemAssuntoDomain = Mapper.Map<ItemAssuntoViewModel, ItemAssunto>(model);
                    db.Entry(ItemAssuntoOriginal).State = EntityState.Detached;
                    db.Entry(ItemAssuntoDomain).State = EntityState.Modified;
                    db.SaveChanges();
                    msg = "O item do assunto foi alterado com sucesso";
                    //var link = "http://localhost:8082/Reuniao/Ata/" + model.ReuniaoId + "";
                    //var texto = "<p>Foi alterado um item que você está como responsavel:&nbsp;" + model.DescricaoItem + " que pertence ao assunto:&nbsp;" + db.Assunto.Find(model.AssuntoId).DescricaoAs + ", para acessar a ATA da &nbsp;<b>Reunião:&nbsp;" + db.reuniao.Find(model.ReuniaoId).Nome + "</b><a href='" + link + "'>&nbsp;Click aqui</p>";
                    // var idresp = _db.Users.Where(u => u.UserName == model.Responsavel).Select(u => u.Id).SingleOrDefault();
                    //var emailresp = _db.Users.Find(idresp).Email;
                    // EmailService emailService = new EmailService();
                    //// await emailService.SendEmailAsync(emailresp, texto, "SGR - Alteração de item");
                    //logCFG.Log("Reunião:" + db.reuniao.Find(ItemAssuntoOriginal.ReuniaoId).Nome + "-ID:" + ItemAssuntoDomain.ReuniaoId + "|Por:" + credencial+ " - Modificacao de item de assunto, de:" + ItemAssuntoOriginal.DescricaoItem + "| para: " + ItemAssuntoDomain.DescricaoItem);
                    return Json(new { success = true, message = msg, ReuniaoId = ItemAssuntoOriginal.ReuniaoId }, JsonRequestBehavior.AllowGet);
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
                ItemAssuntoOriginal = db.itemassunto.Where(a => a.AssuntoId == model.AssuntoId).FirstOrDefault();
                errors.Add("Error occured:" + ex.Message);
                //logCFG.Log("Reunião:" + db.reuniao.Find(AssuntoOriginal.ReuniaoId).Nome + "-ID:" + AssuntoOriginal.ReuniaoId + " - Ocorreu uma tentativa de Modificacao de assunto de:" + AssuntoOriginal.DescricaoAs + "| para: " + model.DescricaoAs + "|Por:" + credencial + "-|Erro:" + errors);
                return Json(new { success = false, message = errors, ReuniaoId = ItemAssuntoOriginal.ReuniaoId }, JsonRequestBehavior.AllowGet);
            }
            
            //logCFG.Log("Reunião:" + db.reuniao.Find(AssuntoOriginal.ReuniaoId).Nome + "-ID:" + AssuntoOriginal.ReuniaoId + " - Ocorreu uma tentativa de Modificacao de assunto de:" + AssuntoOriginal.DescricaoAs + "| para: " + model.DescricaoAs + "|Por:" + credencial + "-|Erro:" + errors);
            return Json(new { success = false, message = errors, ReuniaoId = ItemAssuntoOriginal.ReuniaoId }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListaAssuntos(int ID)
        {
            ContextSGR db_ = new ContextSGR();
            List<Assunto> assuntos = new List<Assunto>();
            foreach (var usr in db.Assunto.Where(u => u.ReuniaoId == ID && u.Delete !=true).ToList())
            {
                assuntos.Add(usr);
            }
            string values = JsonConvert.SerializeObject(assuntos);
            //var json = new JavaScriptSerializer().Serialize(d);
            return Json(new { assuntos }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> Create(ItemAssuntoViewModel model)
        {
            string msg;
            model.InseridoEm = DateTime.Now;
            var errors = new List<string>();
            string credencial = User.Identity.GetUserName();            
            var userreuniao = db.userreuniao.Where(t => t.ReuniaoId == model.ReuniaoId && t.UserName == credencial).SingleOrDefault();
            model.Delete = false;
            model.QuemInseriu = credencial;
            if (ModelState.IsValid && userreuniao.Perfil == "Moderador" && model.Responsavel != "--Selecione--")
            {
                try
                {
                    ItemAssunto ItemAssuntoDomain = Mapper.Map<ItemAssuntoViewModel, ItemAssunto>(model);
                    db.itemassunto.Add(ItemAssuntoDomain);
                    db.SaveChanges();
                    msg = "O item foi inserido no respectivo assunto";
                    //logCFG.Log("Reunião:" + db.reuniao.Find(model.ReuniaoId).Nome + "-ID:" + ItemAssuntoDomain.ReuniaoId + "|Por:" + credencial + " - itemAssunto cadastrado:" + ItemAssuntoDomain.DescricaoItem);
                    //var link = "http://localhost:8082/Reuniao/Ata/" + model.ReuniaoId + "";
                    //var texto = "<p>Você foi designado como responsavel do item:&nbsp;"+model.DescricaoItem+ " que pertence ao assunto:&nbsp;" + db.Assunto.Find(model.AssuntoId).DescricaoAs+ ", para acessar a ATA da&nbsp;<b>Reunião:&nbsp;" + db.reuniao.Find(model.ReuniaoId).Nome + "</b><a href='" + link + "'>&nbsp;Click aqui</p>";
                    //var idresp = _db.Users.Where(u => u.UserName == model.Responsavel).Select(u => u.Id).SingleOrDefault();
                    //var emailresp = _db.Users.Find(idresp).Email;
                    //EmailService emailService = new EmailService();
                    //await emailService.SendEmailAsync(emailresp, texto, "SGR - Nova responsabilidade");
                    return Json(new { success = 1, message = msg, ReuniaoId = model.ReuniaoId }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {

                    errors.Add("Error occured:" + ex.Message);
                    //logCFG.Log("Reunião:" + db.reuniao.Find(model.ReuniaoId).Nome + "-ID:" + model.ReuniaoId + "|Por:" + credencial + " - Ocorreu uma tentativa falha de inserir assunto:" + model.DescricaoItem + "-|Erro:" + errors);
                    return Json(new { success = 0, message = errors, ReuniaoId = model.ReuniaoId });
                }
            }
            else
            {
                if (userreuniao.Perfil != "Moderador")
                {
                    errors.Add("Você não possui privilegio para eecutar essa ação.");
                }
                if (model.Responsavel == "--Selecione--")
                {
                    errors.Add("Você não definil quem será o responsavel.");
                }
                foreach (var modelStateVal in ViewData.ModelState.Values)
                {
                    errors.AddRange(modelStateVal.Errors.Select(error => "</br>" + error.ErrorMessage));
                }
            }

            //logCFG.Log("Reunião:" + db.reuniao.Find(model.ReuniaoId).Nome + "-ID:" + model.ReuniaoId + "|Por:" + credencial + " - Ocorreu uma tentativa fala de inserir assunto:" + model.DescricaoItem + "-|Erro:" + errors);
            return Json(new { success = 0, message = errors, ReuniaoId = model.ReuniaoId });
        }

        [HttpGet]
        public ActionResult ListaItem(int ID)
        {
            List<string> d = new List<string>();
            foreach (var cat in db.itemassunto.Where(r => r.ReuniaoId == ID).ToList())
            {
                d.Add(cat.DescricaoItem);
            }
            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        //Modal demandas itens
        public ActionResult ExibeDemandas(int ID)
        {
            var item = db.itemassunto.Where(d => d.ItemAssuntoId == ID).Include(d=>d.Assunto).SingleOrDefault();
            ViewBag.DemandasQtd = 0;
            ViewBag.DemandasA = 0;
            ViewBag.DemandasS = 0;
            ViewBag.DemandasE = 0;
            if (db.Demanda.Where(d => d.Delete == false && d.ItemAssuntoId == ID).Count()>0)
            {
                ViewBag.DemandasQtd = db.Demanda.Where(d => d.Delete == false && d.ItemAssuntoId == ID).Count();
                if (db.Demanda.Where(d => d.Delete == false && d.ItemAssuntoId == ID && d.Situacao =="Aberto").Count()>0)
                {
                    ViewBag.DemandasA = db.Demanda.Where(d => d.Delete == false && d.ItemAssuntoId == ID && d.Situacao == "Aberto").Count();
                }
                if (db.Demanda.Where(d => d.Delete == false && d.ItemAssuntoId == ID && d.Situacao == "Encerrado").Count() > 0)
                {
                    ViewBag.DemandasE = db.Demanda.Where(d => d.Delete == false && d.ItemAssuntoId == ID && d.Situacao == "Encerrado").Count();
                }
                if (db.Demanda.Where(d => d.Delete == false && d.ItemAssuntoId == ID && d.Situacao == "Suspenso").Count() > 0)
                {
                    ViewBag.DemandasS = db.Demanda.Where(d => d.Delete == false && d.ItemAssuntoId == ID && d.Situacao == "Suspenso").Count();
                }
            }

            ViewBag.Responsavel = new SelectList(_db.Users, "UserName", "UserName", item.Responsavel);
            ViewBag.AssuntoId = new SelectList
            (
                db.Assunto.ToList(),
                "AssuntoId",
                "DescricaoAs", db.Assunto.Where(i => i.AssuntoId == item.AssuntoId).Select(i => i.AssuntoId).SingleOrDefault()
            );
            ViewBag.ItemAssuntoId = new SelectList
            (
                db.itemassunto.ToList(),
                "ItemAssuntoId",
                "DescricaoItem", ID
            );

            ViewBag.Situacao = new SelectList(new[]
            {
                new {Valor = "Aberto", Texto = "Aberto"},
                new {Valor = "Encerrado", Texto = "Encerrado"},
                new {Valor = "Suspenso", Texto = "Suspenso"}
            }, "Valor", "Texto", item.Situacao);

            //return Json(new { item }, JsonRequestBehavior.AllowGet);
            ItemAssuntoViewModel itemview = Mapper.Map<ItemAssuntoViewModel>(item);
            return View(itemview);
        }
    }
}