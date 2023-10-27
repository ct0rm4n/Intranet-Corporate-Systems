using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace IntraNet.Domain.Entities
{
    public class DespesaAnexo
    {
        public int DespesaAnexoId { get; set; }
        public int? RelatorioId { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        public virtual IEnumerable<HttpPostedFileBase> ImageFile { get; set; }
        public virtual Relatorio relatorio { get; set; }
    }
}
