using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SimpleStore.Core.Entities;
using SimpleStore.Core.Entities.Stores;
using SimpleStore.Entities;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SimpleStore.Core.Data
{
    public class StoreDbContext: IdentityDbContext
    {
        private IDbContextTransaction _transaction;

        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Ignore base entity
            builder.Ignore<Entity>();
            builder.Ignore<StoreEntity>();

            // Assembly configuration
            var entitiesAssembly = Assembly.Load("SimpleStore.Core.Entities");
            builder.ApplyConfigurationsFromAssembly(entitiesAssembly);

            // Base
            base.OnModelCreating(builder);
        }

        public async Task BeginTransaction()
        {
            _transaction = await Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            try
            {
                await SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
            }
        }

        public async Task Rollback()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }
    }
}