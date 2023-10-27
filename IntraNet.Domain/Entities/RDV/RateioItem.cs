using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Domain.Entities
{
    public class RateioItem
    {
        public int RateioItemId { get; set; }
        public int SolicitanteCC { get; set; }
        public string Item { get; set; }
        public int UnidadeClasse { get; set; }
        public int RelatorioId { get; set; }
        public int EmpCCustoId { get; set; }
        public string Prospect { get; set; }
        public int Porcentagem { get; set; }

        public virtual Relatorio relatorio { get; set; }
        public virtual EmpCCusto empsCCusto { get; set; }
        [ForeignKey("UnidadeClasse")]
        public virtual Unidade unidade { get; set; }
    }
}
