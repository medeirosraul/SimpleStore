using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleStore.Core.Entities.Payments
{
    public class PaymentMap : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.Property(x => x.Status)
                .HasConversion<string>();
        }
    }

    public class Payment : StoreEntity
    {
        public string OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Value { get; set; }
        public PaymentStatus Status { get; set; }

    }
}
