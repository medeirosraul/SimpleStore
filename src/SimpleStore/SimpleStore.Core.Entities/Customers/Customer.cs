using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleStore.Core.Entities.Carts;
using System;
using System.Collections.Generic;
using System.Security.Permissions;

namespace SimpleStore.Core.Entities.Customers
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasMany(x => x.Addresses)
                .WithOne()
                .HasForeignKey(x => x.CustomerId);
            builder.HasOne(x => x.Cart)
                .WithOne()
                .HasForeignKey<Cart>(x => x.CustomerId);
        }
    }

    public class Customer : StoreEntity
    {
        public string UserId { get; set; }

        // Personal
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string DocumentNumber { get; set; }

        // Navigation
        public virtual Cart Cart { get; set; }
        public virtual ICollection<CustomerAddress> Addresses { get; set; }
    }
}