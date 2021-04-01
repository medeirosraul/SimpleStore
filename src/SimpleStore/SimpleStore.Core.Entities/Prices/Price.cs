using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleStore.Core.Entities.CatalogItems;

namespace SimpleStore.Core.Entities.Prices
{
    public class PriceMap : IEntityTypeConfiguration<Price>
    {
        public void Configure(EntityTypeBuilder<Price> builder)
        {
            builder.ToTable("Prices");
            builder.Property(x => x.Value).IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.Property(x => x.OldValue)
                .HasColumnType("decimal(18,2)");
            builder.Property(x => x.Cost)
                .HasColumnType("decimal(18,2)");
        }
    }

    /// <summary>
    /// General price object for Products and Services
    /// </summary>
    public class Price: StoreEntity
    {
        /// <summary>
        /// Product Id
        /// </summary>
        public string CatalogItemId { get; set; }

        /// <summary>
        /// Actual item value
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Old item value
        /// </summary>
        public decimal OldValue { get; set; }

        /// <summary>
        /// Item cost
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Product related to this price
        /// </summary>
        public virtual CatalogItem CatalogItem { get; set; }
    }
}
