using IntraNet.Domain.Entities.SGR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntraNet.Mod.SGR.Models.ViewModel
{
    public class ReuniaoViewModel
    {
        [Key]
        public int ReuniaoId { get; set; }
        [Required(ErrorMessage = "É obrigatório definir o tema da reunião")]
        public int SetorId { get; set; }
        [Required(ErrorMessage = "É obrigatório definir o nome da reunião")]
        public string Nome { get; set; }
        public string Observac { get; set; }
        public bool Delete { get; set; }
        public DateTime InseridoEm { get; set; }
       
    }
}