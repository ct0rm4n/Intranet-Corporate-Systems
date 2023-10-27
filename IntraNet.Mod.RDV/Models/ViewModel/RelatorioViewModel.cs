using IntraNet.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntraNet.Mod.RDV.Models.ViewModel
{
    public class RelatorioViewModel
    {
        [Key]
        public int RelatorioId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string UserName { get; set; }

        [RegularExpression(@"^.{5,}$", ErrorMessage = "Faça uma descricao mais elaborada")]
        [Required(ErrorMessage = "*Motivo - Você deve justificar o motivo da sua viagem")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Descreva melhor os motivos que levaram a viagem")]
        public string Motivo { get; set; }
        public DateTime? Criacao { get; set; }
        [Required(ErrorMessage = "*Saida - Necessario informar a data de saida")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Saida { get; set; }
        [Required(ErrorMessage = "*Retorno - Necessario informar a data de saida")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Retorno { get; set; }

        public DateTime? Alteracao { get; set; }

        public string Observacoes { get; set; }
        //[Required]
        public int? AprovadorId { get; set; }

        public decimal? AdiantamentoValor { get; set; }
        public int? AdiantamentoId {get;set;}
        public string Situacao { get; set; }

        public int? DadosBancariosId { get; set; }
        [ForeignKey("DadosBancariosId")]
        public virtual DadosBancarios dadosbancarios { get; set; }
        public int? EmpresaId { get; set; }
        [ForeignKey("EmpresaId")]
        public virtual Empresa empresa { get; set; }
        [ForeignKey("AprovadorId")]
        public virtual Aprovador aprovador { get; set; }
        [ForeignKey("AdiantamentoId")]
        public virtual Adiantamento adiantamento { get; set; }
    }
}