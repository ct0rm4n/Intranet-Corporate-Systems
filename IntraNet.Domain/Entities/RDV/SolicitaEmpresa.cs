using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Domain.Entities
{
    public class SolicitaEmpresa
    {
        public int SolicitaEmpresaId { get; set; }
        public int? EmpresaId { get; set; }
        public string UserId { get; set; }
        public string Fornecedor { get; set; }
        public bool Projeto { get; set; }

        public virtual Empresa empresa { get; set; }
    }
}
