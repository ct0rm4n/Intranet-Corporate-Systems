using IntraNet.Security.Models;

namespace IntraNet.Domain.Entities
{
    public class DadosBancarios
    {
        public int DadosBancariosId { get; set; }
        public string UserId { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public int Dv { get; set; }
        public string ContaCorrente { get; set; }
        public string Cpf { get; set; }
    }
}
