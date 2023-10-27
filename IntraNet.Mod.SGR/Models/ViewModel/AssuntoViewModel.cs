using IntraNet.Domain.Entities.SGR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntraNet.Mod.SGR.Models.ViewModel
{
    public class AssuntoViewModel
    {
        [Key]
        public int AssuntoId { get; set; }
        public string Situacao { get; set; }
        [Required(ErrorMessage ="É obrigatório definir uma descrição para o assunto.")]
        public string DescricaoAs { get; set; }
        public int ReuniaoId { get; set; }
        public string QuemInseriu { get; set; }
        public bool Delete { get; set; }
        public DateTime? InseridoEm { get; set; }

        [ForeignKey("ReuniaoId")]
        public virtual Reuniao reuniao { get; set; }
    }
}