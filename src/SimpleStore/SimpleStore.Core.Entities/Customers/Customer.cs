using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleStore.Core.Entities.Carts;

namespace SimpleStore.Core.Entities.Customers
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasOne(x => x.Cart)
                .WithOne()
                .HasForeignKey<Cart>(x => x.CustomerId);
        }
    }

    public class Customer : StoreEntity
    {
        public string UserId { get; set; }

        // Navigation
        public virtual Cart Cart { get; set; }
    }
}