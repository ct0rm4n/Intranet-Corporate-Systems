using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntraNet.Domain.Entities
{
    public class Relatorio
    {
        [Key]
        public int RelatorioId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int EmpresaId { get; set; }
        public string Motivo { get; set; }
        public DateTime? Criacao { get; set; }
        public DateTime Saida { get; set; }
        public DateTime Retorno { get; set; }
        public DateTime? Alteracao { get; set; }
        public string Observacoes { get; set; }
        public int? AprovadorId { get; set; }
        public Decimal? AdiantamentoValor { get; set; }
        public string Situacao { get; set; }
        public int? DadosBancariosId { get; set; }
        public int? AdiantamentoId { get; set; }

        public virtual DadosBancarios dadosbancarios { get; set; }
        public virtual Empresa empresa { get; set; }
        public virtual Aprovador aprovador { get; set; }
        public virtual Adiantamento adiantamento { get; set; }
    }
}
