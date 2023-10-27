using System;
using IntraNet.Domain.Entities;

namespace IntraNet.Domain.Interfaces.Repositories
{
    public interface IRelatorioRepository : IDisposable
    {
        //IEnumerable<Empresa> BuscaPorNomes(string nome);
        Relatorio BuscaPorUsuario(string nome);
    }
}
