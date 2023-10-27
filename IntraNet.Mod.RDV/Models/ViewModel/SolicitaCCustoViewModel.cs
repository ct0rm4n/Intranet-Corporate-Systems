using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IntraNet.Domain.Entities;

namespace IntraNet.Mod.RDV.Models.ViewModel
{
    public class SolicitaCCustoViewModel
    {
        [Key]
        public int SolicitaCCustoId { get; set; }
        public string UserId { get; set; }
        public int EmpCCustoId { get; set; }

        public int SolicitaEmpresaId { get; set; }
        public int? EmpresaId { get; set; }

        [ForeignKey("EmpCCustoId")]
        public virtual EmpCCusto empccusto { get; set; }
        [ForeignKey("SolicitaEmpresaId")]
        public virtual SolicitaEmpresa solicitaempresa { get; set; }
        [ForeignKey("EmpresaId")]
        public virtual Empresa empresa { get; set; }

    }
}