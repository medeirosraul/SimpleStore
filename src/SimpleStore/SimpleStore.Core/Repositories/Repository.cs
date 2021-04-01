using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Entities;
using SimpleStore.Framework.Repositories;
using SimpleStore.Framework.Types;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        private readonly StoreDbContext _context;
        public Repository(StoreDbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> AsQueryable() => AsQueryable(false);

        public IQueryable<TEntity> AsQueryable(bool tracking)
        {
            if (tracking)
                return _context.Set<TEntity>().AsTracking();

            return _context.Set<TEntity>().AsNoTracking();
        }

        public async Task<TEntity> Insert(TEntity entity)
        {
            _context.Add(entity);
            var inserted = await _context.SaveChangesAsync();

            if (inserted > 0) return entity;
            return null;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            _context.Update(entity);
            var updated = await _context.SaveChangesAsync();

            _context.Entry(entity).State = EntityState.Detached;

            if (updated > 0) return entity;
            return null;
        }

        public async Task Delete(TEntity entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
