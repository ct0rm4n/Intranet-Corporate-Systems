using System;
using System.Linq;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities;
using IntraNet.Domain.Interfaces.Repositories;

namespace IntraNet.Data.Repositories
{
    public class DespesaRepository : BaseRepository<Despesas>, IDespesaRepository
    {
        protected readonly ContextRDV _con;
        public DespesaRepository()
        {
            ContextRDV newCTX = new ContextRDV();
            _con = newCTX;
        }

        public void Dispose()
        {
            _con.Dispose();
            GC.SuppressFinalize(this);
        }

        public Despesas BuscaPorNome(string nome)
        {
            return _con.Despesas.Where(c => c.UserId == nome).Single();
        }
    }
}
