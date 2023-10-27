using IntraNet.Data.Context;
using IntraNet.Security.ContextIdentity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IntraNet.Mod.SGR.Controllers
{
    public class TemaController : Controller
    {
        public ContextSGR db = new ContextSGR();
        public ApplicationDbContext dbIdentity = new ApplicationDbContext();
        // GET: Tema
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ListaTemas()
        {
            List<string> d = new List<string>();
            foreach (var cat in dbIdentity.Setor.ToList())
            {
                d.Add(cat.Nome);
            }
            string values = JsonConvert.SerializeObject(d);
            //var json = new JavaScriptSerializer().Serialize(d);
            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }
    }
}