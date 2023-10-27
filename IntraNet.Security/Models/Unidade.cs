using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Security.Models
{
    public class Unidade
    {
        public int UnidadeId { get; set; }
        public string Nome { get; set; }
        public int UnidadeClasse { get; set; }
        public string Estado { get; set; }
        public string Edereco { get; set; }
        public string Cnpj { get; set; }
        public int EmpresaId { get; set; }
        public virtual Empresa empresa { get; set; }
        public bool Ativo { get; set; }
    }
}
