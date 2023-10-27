using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Domain.Entities.SGR
{
    public class ItemAssunto
    {
        public int ItemAssuntoId { get; set; }
        public string DescricaoItem { get; set; }
        public int AssuntoId { get; set; }
        public int ReuniaoId { get; set; }
        public string Situacao { get; set; }
        public string Responsavel { get; set; }
        public DateTime InseridoEm { get; set; }
        public bool Prioridade { get; set; }
        public string QuemInseriu { get; set; }
        public bool Delete { get; set; }

        public Reuniao reuniao { get; set; }
        public Assunto Assunto { get; set; }
    }
}
