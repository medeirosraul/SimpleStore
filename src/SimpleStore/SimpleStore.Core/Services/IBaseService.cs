using SimpleStore.Entities;
using SimpleStore.Framework.Types;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services
{
    public interface IBaseService<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Prepare query with basic filters
        /// </summary>
        /// <param name="tracking">Track entities</param>
        /// <param name="deleted">Include deleted entities</param>
        /// <returns>IQueryable with basic filters</returns>
        IQueryable<TEntity> PrepareQuery(bool tracking = false, bool deleted = false);

        /// <summary>
        /// Verify if entity exists in database
        /// </summary>
        /// <param name="id">Id of entity</param>
        /// <returns>If entity exists</returns>
        Task<bool> Exists(string id);

        /// <summary>
        /// Get entity by Id
        /// </summary>
        /// <param name="id">Entity Id</param>
        /// <param name="tracking">Track entity</param>
        /// <returns>Entity of the specified Id</returns>
        Task<TEntity> GetById(string id, bool tracking = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tracking"></param>
        /// <returns></returns>
        Task<PagedList<TEntity>> Get(bool tracking = false);
        Task<PagedList<TEntity>> Get(IQueryable<TEntity> query , bool tracking = false);
        Task<PagedList<TEntity>> Get(int page, int limit, IQueryable<TEntity> query, bool tracking = false);
        Task<int> InsertOrUpdate(TEntity entity);
        Task<int> Insert(TEntity entity, bool saveChanges = true);
        Task<int> Insert(ICollection<TEntity> entities);
        Task<int> Update(TEntity entity, bool saveChanges = true);
        Task<int> Update(ICollection<TEntity> entities);
        Task<int> Delete(string id, bool soft = true);
        Task<int> Delete(ICollection<TEntity> entities, bool soft = true);
        Task<int> SaveChanges();
    }
}