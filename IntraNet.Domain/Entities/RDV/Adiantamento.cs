using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntraNet.Domain.Entities
{
    public class Adiantamento
    {

        [Key]
        public int AdiantamentoId { get; set; }
        public string UserId { get; set; }
        public DateTime SolicitadoEm { get; set; }
        public DateTime DataPrevista { get; set; }
        public string Situacao { get; set; }
        public Decimal AdiantamentoValor { get; set; }
        public int AprovadorId { get; set; }
        public int DadosBancariosId { get; set; }
        public int? EmpresaId { get; set; }
        public int? SolicitaEmpresaId { get; set; }
        public virtual DadosBancarios dadosbancarios { get; set; }
        public virtual Aprovador aprovador { get; set; }
        public virtual Empresa empresa { get; set; }
        public virtual SolicitaEmpresa solicitaempresa { get; set; }

    }
}
