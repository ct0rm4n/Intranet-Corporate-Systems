using IntraNet.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace IntraNet.Mod.RDV.Models.ViewModel
{
    public class EmpCCustoViewModel
    {
        [Key]
        public int EmpCCustoId { get; set; }
        [Required(ErrorMessage = "É necessario que dê a nomenclatura do Centro de Custo")]
        public string CCusto { get; set; }
        [Required(ErrorMessage = "É necessario que dê a nomenclatura do Centro de Custo")]
        public string CCustoDesc { get; set; }
        [Required(ErrorMessage = "Determine uma empresa")]
        public int EmpresaId { get; set; }
        public bool Projeto { get; set; }
        public bool Prospect { get; set; }
        public int? UnidadeClasse { get; set; }
        public string Item { get; set; }
        public bool Ativo { get; set; }
        [ForeignKey("EmpresaId")]
        public virtual Empresa empresa { get; set; }
        [ForeignKey("UnidadeClasse")]
        public virtual Unidade unidade { get; set; }

    }
}