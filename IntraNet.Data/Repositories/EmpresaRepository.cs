using System;
using System.Linq;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities;
using IntraNet.Domain.Interfaces.Repositories;
namespace IntraNet.Data.Repositories
{
    public class EmpresaRepository : BaseRepository<Empresa>, IEmpresaRepository
    {
        protected readonly ContextRDV _con;
        public EmpresaRepository()
        {
            ContextRDV newCTX = new ContextRDV();
            _con = newCTX;
        }

        public void Dispose()
        {
            _con.Dispose();
            GC.SuppressFinalize(this);
        }

        public Empresa BuscaPorNome(string nome)
        {
            Empresa emp = _con.Empresa.Where(c => c.RazaoSocial == nome).Single();
            return emp;
        }
    }
}
