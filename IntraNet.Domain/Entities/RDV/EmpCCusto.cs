
namespace IntraNet.Domain.Entities
{
    public class EmpCCusto
    {
        public int EmpCCustoId { get; set; }
        //foregin key        
        public int EmpresaId {get; set;}
        public bool? Projeto { get; set; }
        public bool? Prospect { get; set; }
        public string CCusto { get; set; }
        public string CCustoDesc { get; set; }
        public bool Ativo { get; set; }
        public int? UnidadeClasse { get; set; }
        public string Item { get; set; }

        public virtual Empresa empresa { get; set; }
        public virtual Unidade unidade { get; set; }
    }
}
