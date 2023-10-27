using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using IntraNet.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntraNet.Mod.SGR.Models.Identity
{
    public class EditUserViewModel
    {
        [Key]
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "É obrigatório informar a id de usuário")]
        [Display(Name = "Usuário")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "É obrigatório informar um email valido")]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "É obrigatório informar o nome completo do funcionário")]
        [Display(Name = "UseName:")]
        public string FullName { get; set; }
        [Display(Name = "Matricula:")]
        public string Matricula { get; set; }
        [Required(AllowEmptyStrings = true, ErrorMessage = "É obrigatório informar a função")]
        [Display(Name = "Função:")]
        public int? FuncaoId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "É obrigatório informar unidade contratante")]
        [Display(Name = "Unidade:")]
        public int? UnidadeId { get; set; }
        [Required(AllowEmptyStrings = true, ErrorMessage = "É obrigatório informar o setor do funcionário")]
        [Display(Name = "Setor:")]
        public int? SetorId { get; set; }
        [Display(Name = "Num. Telefone:")]
        public string PhoneNumber { get; set; }
        
        [ForeignKey("SetorId")]
        public virtual Setor Setor { get; set; }
        [ForeignKey("UnidadeId")]
        public virtual Unidade Unidade { get; set; }
        [ForeignKey("FuncaoId")]
        public virtual Unidade Funcao { get; set; }


        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}