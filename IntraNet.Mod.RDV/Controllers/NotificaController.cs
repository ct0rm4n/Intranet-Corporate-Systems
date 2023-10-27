using IntraNet.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntraNet.Mod.RDV.Controllers
{
    public class NotificaController : Controller
    {
        private ContextRDV db = new ContextRDV();
        // GET: Notifica
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Painel()
        {
            return View();
        }
        public ActionResult Relatorios()
        {

            return View();
        }
    }
}