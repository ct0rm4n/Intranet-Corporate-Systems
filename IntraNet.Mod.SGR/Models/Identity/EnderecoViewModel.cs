using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntraNet.Mod.SGR.Models.Identity
{
    public class EnderecoViewModel
    {
        public int EnderecoId { get; set; }
        public string UserId { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Rua { get; set; }
    }
}