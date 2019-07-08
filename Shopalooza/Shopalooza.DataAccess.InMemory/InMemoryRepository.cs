using Shopalooza.Core.Contracts;
using Shopalooza.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Shopalooza.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        private ObjectCache _cache = MemoryCache.Default;
        private List<T> _items;
        private string _className;

        public InMemoryRepository()
        {
            _className = typeof(T).Name;
            _items = _cache[_className] as List<T>;
            if (_items == null)
                _items = new List<T>();
        }

        public void Commit()
        {
            _cache[_className] = _items;
        }

        public void Insert(T t)
        {
            _items.Add(t);
        }
        
        public void Update(T t, string id)
        {
            var tToUpdate = _items.Find(b => b.Id == id);

            if (tToUpdate != null)
                tToUpdate = t;
            else
                throw new Exception(_className + " not found");
        }

        public T Find(string id)
        {
           var t = _items.Find(b => b.Id == id);
            if (t != null)
                return t;
            else
                throw new Exception(_className + " not found");
        }

        public IQueryable<T> Collection()
        {
            return _items.AsQueryable();
        }

        public void Delete(string id)
        {
            var tToDelete = _items.Find(b => b.Id == id);

            if (tToDelete != null)
                _items.Remove(tToDelete);
            else
                throw new Exception(_className + " not found");
        }

    }
}
