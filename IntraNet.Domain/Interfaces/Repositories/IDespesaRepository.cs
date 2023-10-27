using System;
using IntraNet.Domain.Entities;

namespace IntraNet.Domain.Interfaces.Repositories
{
    public interface IDespesaRepository : IDisposable
    {
        //IEnumerable<Empresa> BuscaPorNomes(string nome);
        Despesas BuscaPorNome(string nome);
    }
}