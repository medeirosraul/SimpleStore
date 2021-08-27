using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleStore.Core.Entities.Shipping
{
    public class ShipmentMap : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.ToTable("Shipments");

            builder.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey(x => x.ShipmentId);
        }
    }

    public class Shipment : StoreEntity
    {
        public string OrderId { get; set; }
        public string ShippingMethod { get; set; }
        public ShipmentStatus Status { get; set; }

        public string Responsible { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        // Navigations
        public virtual ICollection<ShipmentItem> Items { get; set; }
    }
}
