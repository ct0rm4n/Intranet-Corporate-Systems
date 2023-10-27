using IntraNet.Domain.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntraNet.Mod.RDV.Models.ViewModel
{
    public class DespesaAnexoViewModel
    {
        public int DespesaAnexoId { get; set; }
        public int? RelatorioId { get; set; }
        [DisplayName("Upload File")]
        public string ImagePath { get; set; }
        [NotMapped]
        public virtual IEnumerable<HttpPostedFileBase> ImageFile { get; set; }
        [ForeignKey("RelatorioId")]
        public virtual Relatorio relatorio { get; set; }
    }
}