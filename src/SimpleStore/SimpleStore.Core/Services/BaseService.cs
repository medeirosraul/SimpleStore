using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SimpleStore.Core.Data;
using SimpleStore.Entities;
using SimpleStore.Framework.Types;

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

        public virtual async Task<TEntity> GetById(string id, bool tracking = false)
        {
            return await PrepareQuery(tracking)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public virtual async Task<PagedList<TEntity>> Get(bool tracking = false)
        {
            return await Get(null, tracking);
        }

        public virtual async Task<PagedList<TEntity>> Get(IQueryable<TEntity> query, bool tracking = false)
        {
            query ??= PrepareQuery(tracking);
            return await Get(1, int.MaxValue, query, tracking);
        }

        public virtual async Task<PagedList<TEntity>> Get(int page, int limit, IQueryable<TEntity> query, bool tracking = false)
        {
            query ??= PrepareQuery(tracking);

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
        protected virtual void StampEntry(EntityEntry entry)
        {
            // Stamp the entity
            if(entry.Entity is Entity)
            {
                StampEntryEntity(entry);
            }

            // Stamp the navigation properties of the entity
            if (entry.Navigations.Any())
            {
                foreach (var navigation in entry.Navigations)
                {
                    if (navigation is CollectionEntry)
                    {
                        if (navigation.CurrentValue != null)
                        {
                            foreach (var e in (IEnumerable<Entity>)navigation.CurrentValue)
                            {
                                var navigationEntry = Context.Entry(e);
                                StampEntry(navigationEntry);
                            }
                        }
                    }
                    else if (navigation.CurrentValue is Entity)
                    {
                        var navigationEntry = Context.Entry(navigation.CurrentValue);
                        StampEntry(navigationEntry);
                    }
                }
            }
        }

        protected virtual void StampEntryEntity(EntityEntry entry)
        {
            if (entry.State == EntityState.Added)
            {
                (entry.Entity as Entity).CreatedAt = DateTime.Now;
                (entry.Entity as Entity).ModifiedAt = DateTime.Now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property("CreatedAt").IsModified = false;
                (entry.Entity as Entity).ModifiedAt = DateTime.Now;
            }
        }

        #endregion

        #region Operations

        public virtual async Task<int> InsertOrUpdate(TEntity entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
                return await Insert(entity);
            else
                return await Update(entity);
        }

        public virtual async Task<int> Insert(ICollection<TEntity> entities)
        {
            foreach (var entity in entities)
                await Insert(entity, false);

            return await SaveChanges();
        }

        public virtual async Task<int> Insert(TEntity entity, bool saveChanges = true)
        {
            var entry = Context.Add(entity);

            StampEntry(entry);

            if (saveChanges)
                return await Context.SaveChangesAsync();

            return 0;
        }

        #endregion

        public virtual async Task<int> Update(TEntity entity, bool saveChanges = true)
        {
            // Get entry
            var entry = Context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                // Try get entry for local entity
                var localEntity = Context.Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);

                if (localEntity != null)
                {
                    entry = Context.Entry(localEntity);
                }
                else
                {
                    // Get entry for database entity
                    var databaseEntity = await GetById(entity.Id, true);
                    entry = Context.Entry(databaseEntity);
                }

                // Update entry values
                entry.CurrentValues.SetValues(entity);
            }

            entry.State = EntityState.Modified;

            StampEntry(entry);

            if (saveChanges)
                return await SaveChanges();

            return 0;
        }

        public async Task<int> Update(ICollection<TEntity> entities)
        {
            foreach (var entity in entities)
                await Update(entity, false);

            return await SaveChanges();
        }

        public async Task<int> Delete(string id, bool soft = true)
        {
            var entity = await GetById(id, true);

            if (entity == null) 
                return 0;

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

        public async Task<int> Delete(ICollection<TEntity> entities, bool soft = true)
        {
            var count = 0;
            foreach (var entity in entities)
                count += await Delete(entity.Id, false);

            return count;
        }

        public async Task<int> SaveChanges()
        {
            var changed = await Context.SaveChangesAsync();
            return changed;
        }
    }
}