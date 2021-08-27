using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleStore.Core.Entities.Catalog
{
    public class CatalogItemMap : IEntityTypeConfiguration<CatalogProduct>
    {
        public void Configure(EntityTypeBuilder<CatalogProduct> builder)
        {
            builder.ToTable("CatalogProducts");
            builder.Property(x => x.Name).IsRequired();
            builder.HasMany(x => x.Pictures)
                .WithOne()
                .HasForeignKey(x => x.ProductId);
        }
    }

    public class CatalogProduct : StoreEntity
    {
        public bool Published { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string Sku { get; set; }
        public string Gtin { get; set; }

        #region Pricing
        public decimal Cost { get; set; }
        public decimal OldPrice { get; set; }
        public decimal Price { get; set; }
        #endregion

        #region Inventory
        public int StockQuantity { get; set; }
        public decimal Weight { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Length { get; set; }
        #endregion

        public virtual ICollection<CatalogProductPicture> Pictures { get; set; }
    }
}
