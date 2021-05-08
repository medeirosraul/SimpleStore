using SimpleStore.Entities;
using SimpleStore.Framework.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services
{
    public interface IBaseService<TEntity> where TEntity : Entity
    {
        IQueryable<TEntity> PrepareQuery(bool tracking = false, bool deleted = false);
        Task<bool> Exists(string id);
        Task<TEntity> GetById(string id, bool tracking = false);
        Task<PagedList<TEntity>> Get(bool tracking = false);
        Task<PagedList<TEntity>> Get(IQueryable<TEntity> query , bool tracking = false);
        Task<PagedList<TEntity>> Get(int page, int limit, IQueryable<TEntity> query, bool tracking = false);
        Task<int> InsertOrUpdate(TEntity entity);
        Task<int> Insert(TEntity entity);
        Task<int> Update(TEntity entity);
        Task<int> Delete(string id, bool soft = true);
        Task<int> SaveChanges();
    }
}