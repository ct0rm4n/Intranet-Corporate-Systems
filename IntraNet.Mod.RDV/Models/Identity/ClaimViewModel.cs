using System.ComponentModel.DataAnnotations;


namespace IntraNet.Mod.RDV.Models.Identity
{
    public class ClaimViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Nome da Claim")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Valor da Claim")]
        public string Value { get; set; }
    }
}
