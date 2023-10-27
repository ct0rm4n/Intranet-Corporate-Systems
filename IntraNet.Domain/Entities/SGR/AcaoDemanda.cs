using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Domain.Entities.SGR
{
    public class AcaoDemanda
    {
        public int AcaoDemandaId { get; set; }
        public int ReuniaoId { get; set; }
        public int DemandaId { get; set; }
        public DateTime? Data { get; set; }
        public string Descricao { get; set; }
        public bool Feito { get; set; }
        public string Demandado { get; set; }
        public string QuemInseriu { get; set; }
        public DateTime InseridoEm { get; set; }
        public bool Delete { get; set; }
        public string Observacao { get; set; }

        public virtual Reuniao reuniao { get; set; }
        public virtual Demanda demanda { get; set; }

    }
}
