using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntraNet.Mod.RDV.Models.ViewModel
{
    public class DadosBancariosViewModel
    {
        [Key]
        public int DadosBancariosId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Banco { get; set; }
        [Required]
        public int Agencia { get; set; }
        [Required]
        public int Dv { get; set; }
        [Required]
        public string ContaCorrente { get; set; }
        [Required]
        public string Cpf { get; set; }
    }
}