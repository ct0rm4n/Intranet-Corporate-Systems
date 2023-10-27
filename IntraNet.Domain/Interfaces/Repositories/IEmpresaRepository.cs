using System;
using IntraNet.Domain.Entities;

namespace IntraNet.Domain.Interfaces.Repositories
{
    public interface IEmpresaRepository : IDisposable
    {
        //IEnumerable<Empresa> BuscaPorNomes(string nome);
        Empresa BuscaPorNome(string nome);
    }
}
