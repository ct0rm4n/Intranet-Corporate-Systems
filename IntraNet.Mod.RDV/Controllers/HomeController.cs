using IntraNet.Data.Context;
using IntraNet.Data.Repositories;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace IntraNet.Mod.RDV.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private RelatorioRepository _rep = new RelatorioRepository();
        
        public ActionResult Index()
        {            
            string iduser = User.Identity.GetUserId();
            ViewBag.Total = _rep.GetAll().Where(r => r.UserId == iduser).Count() > 0 ? _rep.GetAll().Where(r => r.UserId == iduser).Count().ToString() : "0";
            ViewBag.Pendente = _rep.GetAll().Where(r => r.UserId == iduser && r.Situacao=="Pendente").Count() > 0 ? _rep.GetAll().Where(r => r.UserId == iduser && r.Situacao == "Pendente").Count().ToString() : "0";
            ViewBag.Aprovado = _rep.GetAll().Where(r => r.UserId == iduser && r.Situacao == "Aprovado").Count() > 0 ? _rep.GetAll().Where(r => r.UserId == iduser && r.Situacao == "Aprovado").Count().ToString().ToString() : "0";
            return View();
        }
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Authorize]
        public ActionResult SignOut()
        {
            ViewBag.Message = "Sair";

            return View();
        }
    }

}
