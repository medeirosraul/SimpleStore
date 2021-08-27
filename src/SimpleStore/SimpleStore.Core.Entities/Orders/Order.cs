using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleStore.Core.Entities.Payments;
using SimpleStore.Core.Entities.Shipping;

namespace SimpleStore.Core.Entities.Orders
{
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.Property(x => x.Status)
                .HasConversion<string>();

            builder.Property(x => x.PaymentStatus)
                .HasConversion<string>();

            builder.HasMany(x => x.ProductItems)
                .WithOne()
                .HasForeignKey(x => x.OrderId);

            builder.HasMany(x => x.Payments)
                .WithOne()
                .HasForeignKey(x => x.OrderId);

            builder.HasMany(x => x.Shipments)
                .WithOne()
                .HasForeignKey(x => x.OrderId);
        }
    }

    public class Order : StoreEntity
    {
        public int OrderNumber { get; set; }

        public string CustomerId { get; set; }

        public OrderStatus Status { get; set; }

        public OrderPaymentStatus PaymentStatus { get; set; }

        public string ShippingMethod { get; set; }

        public decimal ShippingValue { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }

        #region Navigations

        public virtual ICollection<OrderProductItem> ProductItems { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        public virtual ICollection<Shipment> Shipments { get; set; }

        #endregion
    }
}