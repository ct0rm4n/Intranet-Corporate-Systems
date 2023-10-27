using System;
using IntraNet.Domain.Entities;

namespace IntraNet.Domain.Interfaces.Repositories
{
    public interface IDadosBancariosRespository : IDisposable
    {
        //IEnumerable<Empresa> BuscaPorNomes(string nome);
        DadosBancarios BuscaPorNome(string nome);
    }
}