using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntraNet.Mod.SGR.Models.ViewModel
{
    public class FuncaoViewModel
    {
        [Key]
        public int FuncaoId { get; set; }
        [Required(ErrorMessage = "*Insira o nome do departamento!")]
        [MinLength(2, ErrorMessage = "*Insira o nome!")]
        public string Nome { get; set; }
        public string Observacao { get; set; }
        public bool Ativo { get; set; }
    }
}