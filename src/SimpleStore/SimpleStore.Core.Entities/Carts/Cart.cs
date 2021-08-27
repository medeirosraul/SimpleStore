using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            builder.HasMany(x => x.ShippingOptions)
                .WithOne()
                .HasForeignKey(x => x.CartId);
        }
    }

    public class Cart : StoreEntity
    {
        public string CustomerId { get; set; }
        public string ShippingZipCode { get; set; }
        public string SelectedAddress { get; set; }
        public string SelectedPaymentMethod { get; set; }

        // Navigations
        public virtual ICollection<CartItem> Items { get; set; }
        public virtual ICollection<CartShippingOption> ShippingOptions { get; set; }
    }
}