using System;
using System.Linq;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities;
using IntraNet.Domain.Interfaces.Repositories;

namespace IntraNet.Data.Repositories
{
    public class RelatorioRepository : BaseRepository<Relatorio>, IRelatorioRepository
    {
        protected readonly ContextRDV _con;
        public RelatorioRepository()
        {
            ContextRDV newCTX = new ContextRDV();
            _con = newCTX;
        }

        public void Dispose()
        {
            _con.Dispose();
            GC.SuppressFinalize(this);
        }

        public Relatorio BuscaPorUsuario(string nome)
        {
            return _con.Relatorio.Where(c => c.UserName == nome).Single();
        }
    }
}
