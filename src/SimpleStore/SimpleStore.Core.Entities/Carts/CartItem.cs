using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleStore.Core.Entities.Catalog;

namespace SimpleStore.Core.Entities.Carts
{
    public class CartItemMap : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");
            builder.HasOne(x => x.CatalogItem)
                .WithMany()
                .HasForeignKey(x => x.CatalogItemId);
        }
    }

    public class CartItem: StoreEntity
    {
        public string CartId { get; set; }

        public string CatalogItemId { get; set; }

        public int Quantity { get; set; }

        // Navigation
        public CatalogProduct CatalogItem { get; set; }
    }
}