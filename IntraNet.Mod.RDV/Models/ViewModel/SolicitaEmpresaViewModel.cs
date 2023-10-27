using System.ComponentModel.DataAnnotations;
using IntraNet.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntraNet.Mod.RDV.Models.ViewModel
{
    public class SolicitaEmpresaViewModel
    {
        [Key]
        public int SolicitaEmpresaId { get; set; }
        [Required]
        public int EmpresaId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Fornecedor { get; set; }

        [Required]
        public bool Projeto { get; set; }

        [ForeignKey("EmpresaId")]
        public virtual Empresa empresa { get; set; }
    }
}