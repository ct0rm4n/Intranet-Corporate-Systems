using IntraNet.Domain.Entities;
using System;

namespace IntraNet.Domain.Interfaces.Repositories
{
    public interface ISetorEmpRepository : IDisposable
    {
        SetorEmp BuscaPorNome(string nome);
    }
}