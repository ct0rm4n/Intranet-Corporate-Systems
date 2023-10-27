
namespace IntraNet.Domain.Entities
{
    public class Empresa
    {
        public int EmpresaId { get; set; }
        public string RazaoSocial { get; set; }
        public int CodSiga { get; set; }
        public string Complemento { get; set; }
        public bool Ativo { get; set; }
    }
}
