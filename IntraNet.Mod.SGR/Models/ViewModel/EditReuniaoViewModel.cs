using IntraNet.Domain.Entities.SGR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntraNet.Mod.SGR.Models.ViewModel
{
    public class EditReuniaoViewModel
    {
       
        [Key]
        public int ReuniaoId { get; set; }
        [Required(ErrorMessage = "Selecione o setor a qual setor a reunião pertence.")]
        public int SetorId { get; set; }
        [Required(ErrorMessage = "É obrigatório definir o nome da reunião")]
        public string Nome { get; set; }
        public string Observac { get; set; }
        public bool Delete { get; set; }
        public DateTime InseridoEm { get; set; }

        public string[] ListaParticipantes { get; set; }
        [Required(ErrorMessage = "Moderador é indispensavel, ele tera acesso total as informações")]
        public string[] ListaModerador { get; set; }
    }
}