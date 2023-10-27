using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntraNet.Mod.RDV.Models.ViewModel
{
    public class EmpresaViewModel
    {
        [Key]
        public int EmpresaId { get; set; }
        [Required(ErrorMessage = "*Razao Social - Você deve informar o Nome da empresa!")]
        [MaxLength(35, ErrorMessage = "Nome é muito grande")]
        [MinLength(3, ErrorMessage ="Razao Social - Você deve informar um nome maior!")]
        public string RazaoSocial { get; set; }
        [MaxLength(30, ErrorMessage = "Coigo Microsiga - Preencha o codigo do siga")]
        //[Required(ErrorMessage = "*Você deve informar o Codigo do Siga!")]
        public int CodSiga { get; set; }
        public string Complemento { get; set; }
        [Required]
        public bool Ativo { get; set; }
    }
}