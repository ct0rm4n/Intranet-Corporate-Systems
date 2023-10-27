using IntraNet.Domain.Entities.SGR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntraNet.Mod.SGR.Models.ViewModel
{
    public class UserReuniaoViewModel
    {
        [Key]
        public int UserReuniaoId { get; set; }
        public string IdUser { get; set; }
        public int ReuniaoId { get; set; }
        public string Perfil { get; set; }
        public bool Delete { get; set; }

        [ForeignKey("ReuniaoId")]
        public virtual Reuniao reuniao { get; set; }

        public virtual IEnumerable<UserReuniao> ListaParticipantes { get; set; }
        public virtual IEnumerable<UserReuniao> ListaModerador { get; set; }
    }
}