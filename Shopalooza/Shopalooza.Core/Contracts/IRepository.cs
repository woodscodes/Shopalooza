﻿using System.Linq;
using Shopalooza.Core.Models;

namespace Shopalooza.Core.Contracts
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> Collection();
        void Commit();
        void Delete(string id);
        T Find(string id);
        void Insert(T t);
        void Update(T t, string id);
    }
}