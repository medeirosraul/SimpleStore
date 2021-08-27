using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleStore.Core.Entities.Orders;

namespace SimpleStore.Core.Entities.Shipping
{
    public class ShipmentItemMap : IEntityTypeConfiguration<ShipmentItem>
    {
        public void Configure(EntityTypeBuilder<ShipmentItem> builder)
        {
            builder.ToTable("ShipmentItems");

            builder.HasOne(x => x.OrderProductItem)
                .WithOne()
                .HasForeignKey<ShipmentItem>(x => x.OrderProductItemId);
        }
    }
    public class ShipmentItem : StoreEntity
    {
        public string ShipmentId { get; set; }
        public string OrderProductItemId { get; set; }

        #region Navigations

        public virtual OrderProductItem OrderProductItem { get; set; }

        #endregion
    }
}
