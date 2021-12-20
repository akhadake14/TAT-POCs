using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOV.TAT.ProductgRPC.Data
{
    public class GenericRepository<T,C> : IRepository<T> , IDisposable
        where T: class 
        where C : DbContext
    {
        private T entity;
        private C context;

        public GenericRepository(C _context)
        {
            context = _context;
        }
        public T? GetById(int id)
        {
            return context.Set<T>().Find(id);
        }
        public IEnumerable<T> GetAll()
        {
            return context.Set<T>().ToList();
        }
        public void Insert(T entity)
        {
            context.Set<T>().Add(entity);
        }
        public void Delete(int id)
        {
            T entity = context.Set<T>().Find(id);
            if(entity != null)
                context.Set<T>().Remove(entity);
        }
        public void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
