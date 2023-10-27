using AutoMapper;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities.SGR;
using IntraNet.Mod.SGR.Models.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntraNet.Mod.SGR.Controllers
{
    public class GestaoController : Controller
    {
        // GET: Gestao
        public ContextSGR db_ = new ContextSGR();
        public ActionResult Index()
        {
            
            string credencial = User.Identity.GetUserName();

            return View();
        }

        public ActionResult RelatorioDemandasAcoes()
        {
            //ViewBag.ReuniaoId = db_.userdemanda.Where(d=>d.Delete==false && d.UserId == User.Identity.GetUserName())
            ViewBag.ReuniaoId = new SelectList
            (
                db_.reuniao.ToList(),
                "ReuniaoId",
                "Nome"
            );
            ViewBag.Situacao = new SelectList(new[]
            {
                new {Valor = "Aberto", Texto = "Aberto"},
                new {Valor = "Encerrado", Texto = "Encerrado"},
                new {Valor = "Suspenso", Texto = "Suspenso"}
            }, "Valor", "Texto");
            ViewBag.ItemAssuntoId = new SelectList
            (
                db_.itemassunto.ToList(),
                "ItemAssuntoId",
                "DescricaoItem"
            );
            ViewBag.AssuntoId = new SelectList
            (
                db_.Assunto.ToList(),
                "AssuntoId",
                "DescricaoAss"
            );
            return View();
        }

        public ActionResult RelatorioPessoal()
        {
            return View();
        }
        
        public static string PegaHoraAtual()
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;

            int dia = DateTime.Now.Day;
            int ano = DateTime.Now.Year;
            string mes = culture.TextInfo.ToTitleCase(dtfi.GetMonthName(DateTime.Now.Month));
            string diasemana = culture.TextInfo.ToTitleCase(dtfi.GetDayName(DateTime.Now.DayOfWeek));
            string data = diasemana + ", " + dia + " de " + mes + " de " + ano;
            return data;
        }

    }
}