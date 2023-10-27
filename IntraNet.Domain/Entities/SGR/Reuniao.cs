using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Domain.Entities.SGR
{
    public class Reuniao
    {
        public int ReuniaoId { get; set; }
        public int SetorId { get; set; }
        public string Nome  { get; set; }
        public string Observac { get; set; }
        public bool Delete { get; set; }
        public DateTime InseridoEm { get; set; }
    }
}
