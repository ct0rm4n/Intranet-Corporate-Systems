using IntraNet.Data.Context;
using IntraNet.Domain.Entities;
using IntraNet.Domain.Interfaces.Repositories;
using System;
using System.Linq;

namespace IntraNet.Data.Repositories
{
    public class SetorEmpRepository : BaseRepository<SetorEmp>, ISetorEmpRepository
    {
        protected readonly ContextRDV _con;
        public SetorEmpRepository()
        {
            ContextRDV newCTX = new ContextRDV();
            _con = newCTX;
        }

        public void Dispose()
        {
            _con.Dispose();
            GC.SuppressFinalize(this);
        }

        public SetorEmp BuscaPorNome(string nome)
        {
            return _con.SetorEmp.Where(c => c.SetorDesc == nome).FirstOrDefault();
        }
    }
}
