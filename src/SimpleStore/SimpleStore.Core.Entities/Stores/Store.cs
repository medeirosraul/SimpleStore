using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleStore.Core.Entities.Identity;
using SimpleStore.Entities;
using System.Collections.Generic;

namespace SimpleStore.Core.Entities.Stores
{
    public class StoreMap : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            // builder.HasQueryFilter(x => !x.Deleted);
            builder.ToTable("Stores");
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Subdomain).IsRequired();
            builder.HasIndex(x => x.Subdomain).IsUnique();
            builder.HasMany(x => x.Subscriptions)
                .WithOne()
                .HasForeignKey(x => x.StoreId);
        }
    }

    public class Store: Entity
    {
        /// <summary>
        /// Store name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The subdomain of the store
        /// </summary>
        public string Subdomain { get; set; }

        /// <summary>
        /// Host of the store
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Assigned subscriptions
        /// </summary>
        public virtual ICollection<Subscription> Subscriptions { get; set; } 

        public Store()
        {
            Subscriptions = new List<Subscription>();
        }
    }
}