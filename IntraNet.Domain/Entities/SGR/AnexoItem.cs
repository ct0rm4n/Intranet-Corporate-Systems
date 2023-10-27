using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace IntraNet.Domain.Entities.SGR
{
    public class AnexoItem
    {
        public int AnexoItemId { get; set; }
        public int ItemAssuntoId { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string QuemInseriu { get; set; }
        public DateTime Data { get; set; }
        //Caminho
        [NotMapped]
        public HttpPostedFileBase ArquivoFile { get; set; }
        public virtual ItemAssunto ItemAssunto { get; set; }
    }
}
