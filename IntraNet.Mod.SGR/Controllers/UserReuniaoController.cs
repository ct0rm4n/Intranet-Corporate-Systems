using IntraNet.Data.Context;
using IntraNet.Domain.Entities.SGR;
using IntraNet.Security.ContextIdentity;
using IntraNet.Security.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntraNet.Mod.SGR.Controllers
{
    public class UserReuniaoController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        private ContextSGR db_ = new ContextSGR();
        // GET: UserReuniao
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(int Id)
        {
            ViewBag.ReuniaoId = Id;
            ViewBag.UserId = new SelectList(db.Users.OrderBy(x => x.UserName), "Id", "UserName");
            return View();
        }
        [HttpPost]
        //FUNCAO REALIZA CRIAÇÃO DOS USUARIOS DE UMA REUNIÃO
        public ActionResult CreateUser(string[] listarrayU, string[] listarrayM, int ReuniaoId)
        {
            var message = "";

            Reuniao reuniao = db_.reuniao.Find(ReuniaoId);
            
            if (listarrayU != null && listarrayM != null)
            {
                //listarrayu = Lista de usuarios da reuniao com privilegio usuario
                foreach (var item in listarrayU)
                {
                    var user = new ApplicationUser();
                    user = db.Users.Where(n => n.UserName == item).Single();
                    System.Diagnostics.Debug.WriteLine("Id do Usuario a ser inserido é:" + user.Id);
                    System.Diagnostics.Debug.WriteLine("Nome do Usuario a ser inserido é:" + user.UserName);
                    //var nometema = db_.tema.Where(i => i.TemaId == reuniao.TemaId).Single();
                    //System.Diagnostics.Debug.WriteLine("Usuario a ser inserido é:" + item);
                    var users = new UserReuniao()
                    {
                        UserId = user.Id,
                        UserName =  user.UserName,
                        ReuniaoId = reuniao.ReuniaoId,
                        Perfil = "Participante",
                        Delete = false
                    };
                    db_.userreuniao.Add(users);
                    message = "Reunião criada com sucesso!";
                }
                db.SaveChanges();

                foreach (var item in listarrayM)
                {
                    var user = new ApplicationUser();
                    user = db.Users.Where(n => n.UserName == item).Single();
                    //var nometema = db_.tema.Where(i => i.TemaId == reuniao.TemaId).Single();
                    
                    var users = new UserReuniao()
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        ReuniaoId = reuniao.ReuniaoId,
                        Perfil = "Moderador",
                        Delete = false
                    };
                    db_.userreuniao.Add(users);
                }
                db_.SaveChanges();
                return Json(new { message });
            }

            return new EmptyResult();
        }
        //GRIDS FUNCTION
        
        [HttpGet]
        public ActionResult ListaUsuarioReuniao(int ID)
        {
            ContextSGR db_ = new ContextSGR();
            List<string> participantes = new List<string>();
            foreach (var usr in db_.userreuniao.Where(u=>u.ReuniaoId==ID && u.Delete==false).ToList())
            {
                participantes.Add(usr.UserName);
            }
            string values = JsonConvert.SerializeObject(participantes);
            //var json = new JavaScriptSerializer().Serialize(d);
            return Json(new { participantes }, JsonRequestBehavior.AllowGet);
        }
    }
}