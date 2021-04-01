using SimpleStore.Entities;
using SimpleStore.Framework.Types;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Framework.Repositories
{
    public interface IRepository<TEntity>
        where TEntity: Entity
    {
        /// <summary>
        /// Return set as queryable
        /// </summary>
        /// <returns>Queryable</returns>
        IQueryable<TEntity> AsQueryable();

        /// <summary>
        /// Return set as Queryable tracking or not
        /// </summary>
        /// <param name="tracking">Return as tracking?</param>
        /// <returns>Queryable</returns>
        IQueryable<TEntity> AsQueryable(bool tracking);

        /// <summary>
        /// Insert a entity in database
        /// </summary>
        /// <param name="entity">Entity to insert</param>
        /// <returns>Inserted entity</returns>
        Task<TEntity> Insert(TEntity entity);

        /// <summary>
        /// Update a entity in database
        /// </summary>
        /// <param name="entity">Entity to update</param>
        /// <returns>Updated Entity</returns>
        Task<TEntity> Update(TEntity entity);

        /// <summary>
        /// Delete a entity in database
        /// </summary>
        /// <param name="id">Entity identification</param>
        /// <param name="logic">Logical delete</param>
        Task Delete(TEntity id);
    }
}