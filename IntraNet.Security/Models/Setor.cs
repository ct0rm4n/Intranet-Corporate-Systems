using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Security.Models
{
    public class Setor
    {
        public int SetorId { get; set; }
        public string Nome { get; set; }
        public string Observacao { get; set; }
        public bool Ativo { get; set; }
    }
}
