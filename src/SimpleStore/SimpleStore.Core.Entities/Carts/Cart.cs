using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace SimpleStore.Core.Entities.Carts
{
    public class CartMap : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts");
            builder.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey(x => x.CartId);
        }
    }

    public class Cart: StoreEntity
    {
        public string CustomerId { get; set; }

        // Navigations
        public ICollection<CartItem> Items { get; set; }
    }
}
