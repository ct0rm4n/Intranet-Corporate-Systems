using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Domain.Entities.SGR
{
    public class UserReuniao
    {
        public int UserReuniaoId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int ReuniaoId { get; set; }
        public string Perfil { get; set; }
        public bool Delete { get; set; }
        public DateTime InseridoEm { get; set; }

        public virtual Reuniao reuniao { get; set; }
    }
}
