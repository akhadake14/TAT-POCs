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
        public C context;

        public GenericRepository(C _context)
        {
            context = _context;
        }
        public virtual T? GetById(int id)
        {
            return context.Set<T>().Find(id);
        }
        public virtual IEnumerable<T> GetAll()
        {
            return context.Set<T>().AsQueryable();
        }
        public virtual void Insert(T entity)
        {
            context.Set<T>().Add(entity);
        }
        public virtual void Delete(int id)
        {
            T entity = context.Set<T>().Find(id);
            if(entity != null)
                context.Set<T>().Remove(entity);
        }
        public virtual void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
