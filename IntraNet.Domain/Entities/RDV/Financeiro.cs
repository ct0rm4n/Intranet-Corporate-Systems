using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Domain.Entities
{
    public class Financeiro
    {
        public int FinanceiroId { get; set; }
        public string UserId { get; set; }
        public string ArpovName { get; set; }
        public int? EmpresaId { get; set; }
        public string ClaimUser { get; set; }
        public virtual Empresa empresa { get; set; } 
    }
}
