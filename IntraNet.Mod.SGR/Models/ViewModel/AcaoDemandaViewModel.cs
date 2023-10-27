using IntraNet.Domain.Entities.SGR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntraNet.Mod.SGR.Models.ViewModel
{
    public class AcaoDemandaViewModel
    {
        [Key]
        public int AcaoDemandaId { get; set; }
        public int ReuniaoId { get; set; }
        public int DemandaId { get; set; }
        public DateTime? Data { get; set; }
        [Required(ErrorMessage = "Descreva o que foi feito")]
        public string Descricao { get; set; }
        public bool Feito { get; set; }
        public string QuemInseriu { get; set; }
        [Required(ErrorMessage = "Defina quem foi ou será o responsavel pela atividades.")]
        public string Demandado { get; set; }
        public DateTime? InseridoEm { get; set; }
        public bool Delete { get; set; }
        public string Observacao { get; set; }
    }
}