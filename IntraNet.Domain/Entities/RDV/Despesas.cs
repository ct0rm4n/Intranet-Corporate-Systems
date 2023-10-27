using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace IntraNet.Domain.Entities
{
    public class Despesas
    {
        public int DespesasId { get; set; }
        public int TipoDespesaId { get; set; }
        public string Descricao { get; set; }
        public Decimal Valor { get; set; }
        public string Observacao { get; set; }
        public int RelatorioId { get; set; }
        public string UserId { get; set; }

        public virtual Relatorio relatorio { get; set; }
        public virtual TipoDespesa tipodespesa { get; set; }
    }
}
