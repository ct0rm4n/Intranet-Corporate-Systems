using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Domain.Entities
{
    public class Aprovador
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
