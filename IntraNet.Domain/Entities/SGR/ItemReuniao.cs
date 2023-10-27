using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Domain.Entities.SGR
{
    public class ItemReuniao
    {   
        
        public int ItemReuniaoId { get; set; }
        public int ReuniaoId { get; set; }
        public string Grupo { get; set; }

        [ForeignKey("ReuniaoId")]
        public virtual Reuniao reuniao { get; set; }
    }
}
