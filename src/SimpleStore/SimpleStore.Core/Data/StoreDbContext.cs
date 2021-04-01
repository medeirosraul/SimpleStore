using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Entities;
using SimpleStore.Core.Entities.Stores;
using SimpleStore.Entities;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace SimpleStore.Core.Data
{
    public class StoreDbContext: IdentityDbContext
    {
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
    }
}
