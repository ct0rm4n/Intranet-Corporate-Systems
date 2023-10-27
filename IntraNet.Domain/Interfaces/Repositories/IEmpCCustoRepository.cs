using IntraNet.Domain.Entities;
using System;

namespace IntraNet.Domain.Interfaces.Repositories
{
    public interface IEmpCCustoRepository : IDisposable
    {
        EmpCCusto BuscaPorNome(string nome);
    }
}