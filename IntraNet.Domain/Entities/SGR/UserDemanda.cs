using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Domain.Entities.SGR
{
    public class UserDemanda
    {
        public int UserDemandaId { get; set; }
        public string UserId { get; set; }
        public int ReuniaoId { get; set; }
        public int DemandaId { get; set; } 

        public bool Delete { get; set; }
        public DateTime InseridoEm { get; set; }

        public virtual Reuniao reuniao { get; set; }
        public virtual Demanda demanda { get; set; }
    }
}
