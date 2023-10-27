using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using IntraNet.Domain.Entities;

namespace IntraNet.Mod.RDV.Models.ViewModel
{
    public class RateioItemViewModel
    {
        [Key]
        public int RateioItemId { get; set; }
        public int SolicitaCCustoId { get; set; }
        public string Item { get; set; }
        public int UnidadeClasse { get; set; }
        public int RelatorioId { get; set; }
        public int EmpCCustoId { get; set; }
        public string Prospect { get; set; }
        [Required(ErrorMessage = "Dê um valor")]
        [Range(100, int.MaxValue, ErrorMessage = "Não é possivel inserir um valor acima de 100%")]
        public int Porcentagem { get; set; }
        [ForeignKey("RelatorioId")]
        public virtual Relatorio relatorio { get; set; }
        [ForeignKey("EmpCCustoId")]
        public virtual EmpCCusto empsCCusto { get; set; }
        [ForeignKey("SolicitaCCustoId")]
        public virtual SolicitaCCusto solicitacustoid { get; set; }
        [ForeignKey("UnidadeClasse")]
        public virtual Unidade unidade { get; set; }
    }
}