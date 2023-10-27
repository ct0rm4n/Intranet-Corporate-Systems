using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Domain.Entities
{
    public class KMValor
    {
        [Key]
        public int KMValorId { get; set; }
        public int De { get; set; }
        public int Ate { get; set; }
        public decimal Valor { get; set; }
    }
}
