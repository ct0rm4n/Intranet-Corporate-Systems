using IntraNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntraNet.Mod.RDV.Models.ViewModel
{
    public class AdiantamentoViewModel
    {
        [Key]
        public int AdiantamentoId { get; set; }
        public string UserId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? SolicitadoEm { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "Informe quando será a viagem")]
        public DateTime DataPrevista { get; set; }
        public string Situacao { get; set; }
        public int AprovadorId { get; set; }
        [Range(1.0, 1000000000000, ErrorMessage = "Valor invalido, defina quanto deseja solicitar em reais")]
        [Required(ErrorMessage ="Insira o valor necessário")]
        public Decimal AdiantamentoValor { get; set; }
        [Required(ErrorMessage ="Escolha a conta para deposito")]
        public int DadosBancariosId { get; set; }
        public int? EmpresaId { get; set; }
        public int? SolicitaEmpresaId { get; set; }
        public virtual DadosBancarios dadosbancarios { get; set; }
        public virtual Aprovador aprovador { get; set; } 
        public virtual Empresa empresa { get; set; }
        public virtual SolicitaEmpresa solicitaempresa { get; set; }      
    }
}