using IntraNet.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace IntraNet.Mod.RDV.Models.ViewModel
{
    public class AprovadorViewModel
    {
        [Key]
        public int AprovadorId { get; set; }
        public string UserId { get; set; }
        public int? EmpresaId { get; set; }
        public string ArpovName { get; set; }
        public string Local { get; set; }
        public string ClaimUser { get; set; }
        public int? SetorId { get; set; }
        public bool? Moderador { get; set; }

        public virtual Empresa empresa { get; set; }
        public virtual Setor setor { get; set; }
    }
}