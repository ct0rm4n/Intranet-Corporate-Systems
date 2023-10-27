using System.ComponentModel.DataAnnotations;
using IntraNet.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntraNet.Mod.RDV.Models.ViewModel
{
    public class UnidadeViewModel
    {
        [Key]
        public int UnidadeId { get; set; }
        [Required(ErrorMessage = "*Insira o nome da unidade!")]
        [MinLength(2, ErrorMessage = "*Tamanho minimo de {0} digitos!")]
        public string Nome { get; set; }
        public int UnidadeClasse { get; set; }
        public string Estado { get; set; }
        public string Edereco { get; set; }
        public string Cnpj { get; set; }
        public int EmpresaId { get; set; }
        [ForeignKey("EmpresaId")]
        public virtual Empresa empresa { get; set; }
    }
}