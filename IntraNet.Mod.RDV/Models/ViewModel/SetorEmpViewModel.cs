using IntraNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntraNet.Mod.RDV.Models.ViewModel
{
    public class SetorEmpViewModel
    {
        [Key]
        public int SetorEmpId { get; set; }
        public string SetorDesc { get; set; }
        public int SetorId { get; set; }
        public int EmpresaId { get; set; }
        [ForeignKey("EmpresaId")]
        public virtual Empresa empresa { get; set; }
        [ForeignKey("SetorId")]
        public virtual Setor Setor { get; set; }
    }
}