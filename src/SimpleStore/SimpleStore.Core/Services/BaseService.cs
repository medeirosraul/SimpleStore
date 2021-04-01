using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Entities;
using SimpleStore.Framework.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : Entity
    {
        protected readonly StoreDbContext Context;

        public BaseService(StoreDbContext context)
        {
            Context = context;
        }

        #region Queries
        public virtual IQueryable<TEntity> PrepareQuery(bool tracking = false, bool deleted = false)
        {
            var prepared = tracking ? Context.Set<TEntity>().AsTracking() : Context.Set<TEntity>().AsNoTracking();

            if(!deleted)
            {
                prepared = prepared.Where(p => !p.Deleted);
            }

            return prepared;
        }

        public virtual async Task<bool> Exists(string id)
        {
            return await PrepareQuery()
                .AnyAsync(p => p.Id == id);
        }

        public virtual async Task<TEntity> GetById(string id)
        {
            return await PrepareQuery()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public virtual async Task<PagedList<TEntity>> Get()
        {
            return await Get(null);
        }

        public virtual async Task<PagedList<TEntity>> Get(IQueryable<TEntity> query)
        {
            query ??= PrepareQuery();
            return await Get(1, int.MaxValue, query);
        }

        public virtual async Task<PagedList<TEntity>> Get(int page, int limit, IQueryable<TEntity> query)
        {
            query ??= PrepareQuery();

            // Count
            var count = await query.CountAsync();

            // Create result
            var result = new PagedList<TEntity>
            {
                TotalCount = count,
                PageIndex = page,
                PageSize = limit
            };

            // Return if count = 0
            if (count == 0) return result;

            // Paginate
            query = limit == 0 ? query : query.Skip((page - 1) * limit).Take(limit);

            // Return result
            result.AddRange(await query.ToListAsync());

            return result;
        }
        #endregion

        #region Prepare
        public virtual void TrackEntity(IEnumerable<TEntity> entities)
        {
            foreach(var e in entities)
            {
                TrackEntity(e);
            }
        }

        public virtual void TrackEntity(TEntity entity)
        {
            Stamp(entity);

            var entry = Context.Entry(entity);

            if (entry.State == EntityState.Detached && string.IsNullOrEmpty(entity.Id))
                entry = Context.Set<TEntity>().Add(entity);
            else if(entry.State == EntityState.Detached)
            {
                entry = Context.Set<TEntity>().Update(entity);
            }

            if (entry.State == EntityState.Modified)
                entry.Property(p => p.CreatedAt).IsModified = false;
        }

        /// <summary>
        /// Stamp entity metadata with basic informations
        /// </summary>
        /// <param name="entity">Returns stamped entity</param>
        protected virtual void Stamp(TEntity entity)
        {
            entity.ModifiedAt = DateTime.Now;
            if (string.IsNullOrEmpty(entity.Id))
                entity.CreatedAt = DateTime.Now;
        }

        #endregion

        public virtual async Task<int> InsertOrUpdate(TEntity entity)
        {
            TrackEntity(entity);
            return await SaveChanges();
        }

        public async Task<int> Delete(string id, bool soft = true)
        {
            var entity = await GetById(id);
            if (entity == null) return 0;

            if (soft)
            {
                entity.Deleted = true;
                await InsertOrUpdate(entity);
            }
            else
            {
                Context.Set<TEntity>().Remove(entity);
            }

            return await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            var changed = await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();
            return changed;
        }
    }
}
