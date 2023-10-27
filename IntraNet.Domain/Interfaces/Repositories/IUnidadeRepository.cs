using System;
using System.Collections.Generic;
using IntraNet.Domain.Entities;

namespace IntraNet.Domain.Interfaces.Repositories
{
    public interface IUnidadeRepository : IDisposable
    {
        IEnumerable<Unidade> BuscaPorNome(string nome);
    }
}
