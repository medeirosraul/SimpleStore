using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleStore.Core.Entities.Catalog;

namespace SimpleStore.Core.Entities.Orders
{
    public class OrderProductItemMap : IEntityTypeConfiguration<OrderProductItem>
    {
        public void Configure(EntityTypeBuilder<OrderProductItem> builder)
        {
            builder.ToTable("OrderProductItems");

            builder.HasOne(x => x.CatalogProduct)
                .WithMany()
                .HasForeignKey(x => x.CatalogProductId);
        }
    }

    public class OrderProductItem : StoreEntity
    {
        public string OrderId { get; set; }

        public string CatalogProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        // Navigations
        public CatalogProduct CatalogProduct { get; set; }
    }
}
