using IntraNet.Domain.Entities.SGR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntraNet.Mod.SGR.Models.ViewModel
{
    public class ItemAssuntoViewModel
    {
        [Key]
        public int ItemAssuntoId { get; set; }
        public int AssuntoId { get; set; }
        public int ReuniaoId { get; set; }
        public string Situacao { get; set; }
        [Required(ErrorMessage ="É obrigatório definir um responsavel")]
        public string Responsavel { get; set; }
        [Required(ErrorMessage = "É obrigatório descrever o item")]        
        public string DescricaoItem { get; set; }
        public bool Delete { get; set; }
        public DateTime? InseridoEm { get; set; }
        public bool Prioridade { get; set; }
        public string QuemInseriu { get; set; }        
    }
}