using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntraNet.Mod.SGR.Models.Identity
{
    public class EmpresaCreateViewModel
    {
        [Key]
        public int EmpresaId { get; set; }
        public string RazaoSocial { get; set; }
        public int CodSiga { get; set; }
        public string Complemento { get; set; }
        public bool Ativo { get; set; }
    }
}