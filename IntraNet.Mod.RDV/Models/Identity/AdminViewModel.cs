using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using IntraNet.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntraNet.Mod.RDV.Models.Identity
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Matricula:")]
        public string Matricula { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "E-mail:")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "User Name:")]
        public string Name { get; set; }
        
        [Display(Name = "Unidade:")]
        public int UnidadeId { get; set; }

        [Display(Name = "Setor:")]
        public int SetorId { get; set; }

        [Display(Name = "Num. Telefone:")]
        public string PhoneNumber { get; set; }

       
        //[ForeignKey("Local")]
        public virtual Setor Setor { get; set; }
        public virtual Unidade Unidade { get; set; }
        public IEnumerable<SelectListItem> RolesList { get; set; }
    }

   
}