using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntraNet.Mod.SGR.Controllers
{
    public class LogController : Controller
    {
        // GET: Log
        public ActionResult Index()
        {
            return View();
        }

        public void Log(string mensagem)
        {
            string arquivo = @"C:\Projetos\IntraNetVT\IntraNet.Mod.SGR\loglog.txt";
            System.IO.StreamWriter file = new System.IO.StreamWriter(arquivo, true, System.Text.Encoding.Default);
            file.WriteLine("SGR ->"+DateTime.Now + ">-"+ mensagem);
            file.Dispose();
        }
    }
}