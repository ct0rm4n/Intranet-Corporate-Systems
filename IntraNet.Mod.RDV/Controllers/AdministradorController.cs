using IntraNet.Data.Repositories;
using System.Web.Mvc;

namespace IntraNet.Mod.RDV.Controllers
{
    [AllowAnonymous]
    public class AdministradorController : Controller
    {
        private EmpresaRepository _rep = new EmpresaRepository();
        
        // GET: Administrador
        //[Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ControleEmp()
        {
            return View();
        }
    }
}