using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using IntraNet.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntraNet.Mod.RDV.Models.ViewModel
{
    public class SetorViewModel
    {
        [Key]
        public int SetorId { get; set; }

        [Required(ErrorMessage = "*Insira o nome do departamento!")]
        [MinLength(2, ErrorMessage = "*Insira o nome!")]
        public string Nome { get; set; }
        public string Observacao { get; set; }
        
    }
}