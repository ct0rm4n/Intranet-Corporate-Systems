using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntraNet.Mod.SGR.Models.ViewModel
{
    public class AnexoItemViewModel
    {
        [Key]
        public int FileId { get; set; }
        public int ItemAssuntoId { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string QuemInseriu { get; set; }
        public DateTime Data { get; set; }
        public bool Delete { get; set; }
        //Caminho
        [NotMapped]
        public HttpPostedFileBase ArquivoFile { get; set; }
    }
}