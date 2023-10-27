using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Security.Models
{
    public class Empresa
    {
        public int EmpresaId { get; set; }
        public string RazaoSocial { get; set; }
        public int CodSiga { get; set; }
        public string Complemento { get; set; }
        public bool Ativo { get; set; }
    }
}
