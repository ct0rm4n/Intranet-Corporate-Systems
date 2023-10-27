using System;
using System.Collections.Generic;
using System.Linq;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities;
using IntraNet.Domain.Interfaces.Repositories;

namespace IntraNet.Data.Repositories
{
    public class UnidadeRepository : BaseRepository<Unidade>, IUnidadeRepository
    {
        protected readonly ContextRDV _con;
        public UnidadeRepository()
        {
            ContextRDV newCTX = new ContextRDV();
            _con = newCTX;
        }

        public void Dispose()
        {
            _con.Dispose();
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Unidade> BuscaPorNome(string nome)
        {
            return _con.Unidade.Where(c => c.Nome == nome);
        }
    }
}
