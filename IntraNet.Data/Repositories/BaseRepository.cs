using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntraNet.Data.Context;
using IntraNet.Domain.Interfaces.Repositories;

namespace IntraNet.Data.Repositories
{
    public class BaseRepository<TEntidade> : IBaseRepository<TEntidade> where TEntidade : class
    {
        protected readonly ContextRDV _con;

        public BaseRepository()
        {
            ContextRDV newCTX = new ContextRDV();
            _con = newCTX;
        }
        public IQueryable<TEntidade> GetAll()
        {
            return _con.Set<TEntidade>();
        }

        public void Alterar(TEntidade obj)
        {
            _con.Entry(obj).State = EntityState.Detached;
            _con.Set<TEntidade>().AddOrUpdate(obj);
            //_con.Entry(obj).State = EntityState.Modified;
            _con.SaveChanges();
        }

        public void Inserir(TEntidade obj)
        {
            try
            {
                _con.Set<TEntidade>().Add(obj);
                _con.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public TEntidade RecuperarPorID(int Id)
        {
            return _con.Set<TEntidade>().Find(Id);
        }

        public IList<TEntidade> RecurperarTodos()
        {
            return _con.Set<TEntidade>().ToList();
        }

        public void Remover(TEntidade obj)
        {
            _con.Set<TEntidade>().Remove(obj);
            _con.SaveChanges();
        }
    }
}
