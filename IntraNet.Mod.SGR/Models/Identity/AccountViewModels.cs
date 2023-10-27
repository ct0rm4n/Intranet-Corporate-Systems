using IntraNet.Security.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntraNet.Mod.SGR.Models.Identity
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Sobrenome")]
        public string LastName { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }

        [HiddenInput]
        public string UserId { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Código")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Lembrar este Browser?")]
        public bool RememberBrowser { get; set; }

        [HiddenInput]
        public string UserId { get; set; }

    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Lembrar login?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel : EnderecoViewModel
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "É obrigatório informar a id de usuário")]
        [Display(Name = "Usuário")]
        public string UserName { get; set; }

        [Required( ErrorMessage = "É obrigatório informar um email valido")]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "É obrigatório informar o nome completo do funcionário")]
        [Display(Name = "UseName:")]
        public string FullName { get; set; }

        [Display(Name = "Matricula:")]
        public string Matricula { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "É obrigatório informar a função")]
        [Display(Name = "Função:")]
        public int? FuncaoId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "É obrigatório informar unidade contratante")]
        [Display(Name = "Unidade:")]
        public int? UnidadeId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "É obrigatório informar o setor do funcionário")]
        [Display(Name = "Setor:")]
        public int? SetorId { get; set; }

        [Display(Name = "Num. Telefone:")]
        public string PhoneNumber { get; set; }
        

        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "As senhas não se coincidem.")]
        public string ConfirmPassword { get; set; }
        public virtual EnderecoViewModel Endereco { get; set; }
        [ForeignKey("SetorId")]
        public virtual Setor Setor { get; set; }
        [ForeignKey("UnidadeId")]
        public virtual Unidade Unidade { get; set; }
        [ForeignKey("FuncaoId")]
        public virtual Unidade Funcao{get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "As senhas não se coincidem.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
    
}