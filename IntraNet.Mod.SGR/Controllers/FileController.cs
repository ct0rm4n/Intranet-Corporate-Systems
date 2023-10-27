using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntraNet.Mod.SGR.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateImg()
        {
            return View();
        }
        public JsonResult CreateImgUpload()
        {
            var message = "";
            try {

                message = "Inserido com sucesso";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);

            }
            catch(Exception e)
            {
                message = "Ocorreu um erro ao inserir";
                return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
            };
            return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
        }


    }
}