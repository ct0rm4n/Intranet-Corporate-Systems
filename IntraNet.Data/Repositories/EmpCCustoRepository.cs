using IntraNet.Data.Context;
using IntraNet.Domain.Entities;
using IntraNet.Domain.Interfaces.Repositories;
using System;
using System.Linq;

namespace IntraNet.Data.Repositories
{
    public class EmpCCustoRepository : BaseRepository<EmpCCusto>, IEmpCCustoRepository
    {
        protected readonly ContextRDV _con;
        public EmpCCustoRepository()
        {
            ContextRDV newCTX = new ContextRDV();
            _con = newCTX;
        }

        public void Dispose()
        {
            _con.Dispose();
            GC.SuppressFinalize(this);
        }

        public EmpCCusto BuscaPorNome(string nome)
        {
            return _con.EmpresaCC.Where(c => c.CCustoDesc == nome).Single();
        }
    }
}
