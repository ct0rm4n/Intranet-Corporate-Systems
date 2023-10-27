using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Domain.Entities
{
    public class SolicitaCCusto
    {
        public int SolicitaCCustoId { get; set; }
        public string UserId { get; set; }

        public int EmpCCustoId { get; set; }
        public int SolicitaEmpresaId { get; set; }
        public int? EmpresaId { get; set; }

        public virtual EmpCCusto empccusto { get; set; }
        public virtual SolicitaEmpresa solicitaempresa { get; set; }
        public virtual Empresa empresa { get; set; }
    }
}
