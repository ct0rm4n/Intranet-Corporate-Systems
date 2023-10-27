using System;
using IntraNet.Domain.Entities;

namespace IntraNet.Domain.Interfaces.Repositories
{
    public interface ISetorRepository : IDisposable
    {
        Setor BuscaPorNome(string nome);
    }
}
