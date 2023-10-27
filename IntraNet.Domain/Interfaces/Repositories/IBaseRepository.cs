using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<TEntidade> where TEntidade : class
    {
        IList<TEntidade> RecurperarTodos();
        TEntidade RecuperarPorID(int Id);
        void Inserir(TEntidade obj);
        void Remover(TEntidade obj);
        void Alterar(TEntidade obj);
        IQueryable<TEntidade> GetAll();
    }
}
