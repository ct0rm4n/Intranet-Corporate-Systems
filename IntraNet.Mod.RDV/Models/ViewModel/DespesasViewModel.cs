using IntraNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntraNet.Mod.RDV.Models.ViewModel
{
    public class DespesasViewModel
    {
        [Key]
        public int DespesasId { get; set; }
        [Required(ErrorMessage = "Determine o tipo da despesa")]
        public int TipoDespesaId { get; set; }
        [Required(ErrorMessage = "De uma descrição")]
        public string Descricao { get; set; }
        public string Observacao { get; set; }
        [Required(ErrorMessage = "Dê um valor")]
        [DisplayFormat(DataFormatString = "{0:$###.##}")]
        public Decimal Valor { get; set; }
        public int RelatorioId { get; set; }
        public string UserId { get; set; }
        //[DisplayName("Upload File")]
        //public string ImagePath { get; set; }
        //[NotMapped]
        //public virtual IEnumerable<HttpPostedFileBase> ImageFile { get; set; }
        [ForeignKey("RelatorioId")]
        public virtual Relatorio relatorio { get; set; }
        [ForeignKey("TipoDespesaId")]
        public virtual TipoDespesa tipodespesa { get; set; }
    }
}