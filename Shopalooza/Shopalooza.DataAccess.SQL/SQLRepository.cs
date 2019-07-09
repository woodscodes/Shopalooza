using Shopalooza.Core.Contracts;
using Shopalooza.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopalooza.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        private DataContext _context;
        private DbSet<T> _dbSet;

        public SQLRepository(DataContext dataContext)
        {
            _context = dataContext;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> Collection()
        {
            return _dbSet;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            var tToDelete = Find(id);
            if (_context.Entry(tToDelete).State == EntityState.Detached)
                _dbSet.Attach(tToDelete);

            _dbSet.Remove(tToDelete);
        }

        public T Find(string id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(T t)
        {
            _dbSet.Add(t);
        }

        public void Update(T t, string id)
        {
            _dbSet.Attach(t);
            _context.Entry(t).State = EntityState.Modified;
        }
    }
}
