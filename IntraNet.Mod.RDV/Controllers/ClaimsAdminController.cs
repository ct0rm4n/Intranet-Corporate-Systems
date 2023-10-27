using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using IntraNet.Security.ContextIdentity;
using IntraNet.Security.Filters;
using IntraNet.Security.Identity;
using IntraNet.Security.Models;
using IntraNet.Mod.RDV.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using IntraNet.Domain.Entities;
using IntraNet.Data.Context;

namespace IntraNet.Mod.RDV.Controllers
{
    //[ClaimsAuthorize("AdmClaims", "True")]
    //[Authorize(Roles = "Admin")]
    [AllowAnonymous]
    public class ClaimsAdminController : Controller
    { 
        public ClaimsAdminController()
        {
        }

        public ClaimsAdminController(ApplicationUserManager userManager, ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            set { _userManager = value; }
        }

        private ApplicationDbContext _dbContext;

        public ApplicationDbContext DbContext
        {
            get { return _dbContext ?? HttpContext.GetOwinContext().GetUserManager<ApplicationDbContext>(); }
            set { _dbContext = value; }
        }

        // GET: ClaimsAdmin
        public ActionResult Index()
        {
            return View(DbContext.Claims.ToList());
        }

        // GET: ClaimsAdmin/SetUserClaim
        public ActionResult SetUserClaim(string id)
        {
            ViewBag.Name = new SelectList
                (
                    DbContext.Claims.ToList(),
                    "Name",
                    "Name"
                );

            ViewBag.User = UserManager.FindById(id);

            return View();
        }

        // POST: ClaimsAdmin/SetUserClaim
        [HttpPost]
        public ActionResult SetUserClaim(ClaimViewModel claim, string id)
        {
            ContextRDV db = new ContextRDV();
            var username = UserManager.FindById(id).UserName;
            //var empresaid = UserManager.FindById(id).Local;
            var setorid = UserManager.FindById(id).Setor;
            try
            {
                
                UserManager.AddClaim(id, new Claim(claim.Name, claim.Value));
                db.SaveChanges();
                //adiciona aos perfis
                if (claim.Name == "Aprovador")
                {
                    Aprovador aprov = new Aprovador()
                    {
                        Local = claim.Value,
                        ClaimUser = claim.Id,
                        UserId = id,
                        ArpovName = username,
                        Moderador= false,
                    };
                    db.Aprovador.Add(aprov);
                    db.SaveChanges();
                }
                else if (claim.Name == "Moderador")
                {
                    Aprovador aprov = new Aprovador()
                    {
                        Local = claim.Value,
                        ClaimUser = claim.Id,
                        UserId = id,
                        ArpovName = username,
                        Moderador = true,
                    };
                    db.Aprovador.Add(aprov);
                    db.SaveChanges();
                }
                else if(claim.Name == "Financeiro")
                {
                    Financeiro financ = new Financeiro()
                    {
                        ClaimUser = claim.Id,
                        UserId = id,
                        ArpovName = username                        
                    };
                    db.Financeiro.Add(financ);
                    db.SaveChanges();
                }
                db.SaveChangesAsync();
                return RedirectToAction("Details", "UsersAdmin", new { id = id });
            }
            catch
            {
                return View();
            }
        }

        // GET: ClaimsAdmin/CreateClaim
        public ActionResult CreateClaim()
        {
            return View();
        }

        // POST: ClaimsAdmin/CreateClaim
        [HttpPost]
        public ActionResult CreateClaim(Claims claim)
        {
            try
            {
                var ultimo = DbContext.Claims.Count() + 1;
                claim.Id = ultimo;
                if (ModelState.IsValid)
                {                    
                    DbContext.Claims.Add(claim);
                    DbContext.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //var er = "erro";
                return View(ex);
            }
        }
    }
}
