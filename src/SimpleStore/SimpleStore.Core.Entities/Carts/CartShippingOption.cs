using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleStore.Core.Entities.Carts
{
    public class CartShippingOptionMap : IEntityTypeConfiguration<CartShippingOption>
    {
        public void Configure(EntityTypeBuilder<CartShippingOption> builder)
        {
            builder.ToTable("CartShippinhOptions");
        }
    }

    public class CartShippingOption : StoreEntity
    {
        public string CartId { get; set; }
        public string Method { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public bool Selected { get; set; }
    }
}