using System;
using System.Linq;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities;
using IntraNet.Domain.Interfaces.Repositories;

namespace IntraNet.Data.Repositories
{
    public class SetorRepository : BaseRepository<Setor>, ISetorRepository
    {
        protected readonly ContextRDV _con;
        public SetorRepository()
        {
            ContextRDV newCTX = new ContextRDV();
            _con = newCTX;
        }

        public void Dispose()
        {
            _con.Dispose();
            GC.SuppressFinalize(this);
        }

        public Setor BuscaPorNome(string nome)
        {
            return _con.Setor.Where(c => c.Nome == nome).Single();
        }
    }
}
