
namespace IntraNet.Domain.Entities
{
    public class SetorEmp
    {
        public int SetorEmpId { get; set; }
        public string SetorDesc { get; set; }
        public int SetorId { get; set; }
        public int EmpresaId { get; set; }

        public virtual Empresa empresa { get; set; }
        public virtual Setor setor { get; set; }
    }
}
