using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Domain.Entities.SGR
{
    public class Assunto
    {
        public int AssuntoId { get; set; }
        public string Situacao { get; set; }
        public string DescricaoAs { get; set; }
        public int ReuniaoId { get; set; }
        public string QuemInseriu { get; set; }
        public bool Delete { get; set; }
        public DateTime InseridoEm { get; set; }
        //public virtual Grupo grupo
        public virtual Reuniao reuniao { get; set; }
    }
}
