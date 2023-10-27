using IntraNet.Domain.Entities.SGR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntraNet.Mod.SGR.Models.ViewModel
{
    public class DemandaViewModel
    {
        [Key]
        public int DemandaId { get; set; }
        public int ReuniaoId { get; set; }
        public int ItemAssuntoId { get; set; }
        public string Situacao { get; set; }
        [Required(ErrorMessage = "O quê? (What) - Deescreva o conceito da principal da demanda.")]
        public string Oque { get; set; }
        public string Como { get; set; }
        public string Porque { get; set; }
        public string Quanto { get; set; }
        public string Onde { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Até quando? (When) - Data estipulada para resolução.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Quando { get; set; }
        
        [Required(ErrorMessage = "*Data para a resolução da demanda.")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yy-MM-dd}")]
        public DateTime InseridoEm { get; set; }
        public bool Delete { get; set; }
        public string QuemInseriu { get; set; }
        [Required(ErrorMessage = "Quem ? (Whom) - Quem está delagando essa demanda.")]
        public string Quem { get; set; }
        
        public virtual IEnumerable<UserDemanda> Demandado { get; set; }

        [ForeignKey("ItemAssuntoId")]
        public virtual ItemAssunto itemassunto { get; set; }
        [ForeignKey("ReuniaoId")]
        public virtual Reuniao reuniao { get; set; }
    }
}