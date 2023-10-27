using AutoMapper;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities;
using IntraNet.Mod.RDV.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;


namespace IntraNet.Mod.RDV.Controllers
{
    public class DespesasAnexoController : Controller
    {
        // GET: DespesasAnexo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManegeImages(int id)
        {
            return View();
        }
        public ActionResult ListImages(int id)
        {
            ContextRDV db = new ContextRDV();
            List<DespesaAnexo> desp = db.DespesaAnexo.Where(da=>da.RelatorioId ==id).ToList();
            var DespesasAnexosDomain = Mapper.Map<List<DespesaAnexo>,List<DespesaAnexoViewModel>>(desp);
            return View(DespesasAnexosDomain);
        }
        [HttpPost]
        public JsonResult Create(DespesaAnexoViewModel despesas)
        {
            ContextRDV db = new ContextRDV();
            foreach (var ImageFile in despesas.ImageFile)
            {
                if (ImageFile.ContentLength > 0)
                {
                    DespesaAnexo desp = new DespesaAnexo();
                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    fileName = fileName+extension;
                    desp.ImagePath = "/Relatorios/Despesas/" + fileName;                
                    desp.RelatorioId = despesas.RelatorioId;
                    fileName = HttpContext.Server.MapPath("~/Relatorios/Despesas/" + fileName);
                    ImageFile.SaveAs(fileName);
                    db.DespesaAnexo.Attach(desp);
                    db.DespesaAnexo.Add(desp);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Comprovante inserido com sucesso" }, JsonRequestBehavior.AllowGet);
                }
                
            }
            return Json(new { success = false, message = "Não foi possivel inserir o comprovante." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Slider(int id)
        {
            ContextRDV db = new ContextRDV();
            var imagens = db.DespesaAnexo.Where(r => r.RelatorioId == id);
            ViewBag.Despesas = imagens;
            return View();
        }
    }
}