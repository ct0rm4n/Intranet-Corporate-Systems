using System;
using System.Linq;
using IntraNet.Data.Context;
using IntraNet.Domain.Entities;
using IntraNet.Domain.Interfaces.Repositories;

namespace IntraNet.Data.Repositories
{
    class DadosBancarioRepository : BaseRepository<DadosBancarios>, IDadosBancariosRespository
    {

        protected readonly ContextRDV _con;
        public DadosBancarioRepository()
        {
            ContextRDV newCTX = new ContextRDV();
            _con = newCTX;
        }

        public void Dispose()
        {
            _con.Dispose();
            GC.SuppressFinalize(this);
        }

        public DadosBancarios BuscaPorNome(string nome)
        {
            return _con.DadosBancarios.Where(c => c.UserId == nome).Single();
        }
    }
}
