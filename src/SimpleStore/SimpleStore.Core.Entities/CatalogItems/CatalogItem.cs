using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpleStore.Core.Entities.Prices;
using System.Collections.Generic;

namespace SimpleStore.Core.Entities.CatalogItems
{
    public class CatalogItemMap : IEntityTypeConfiguration<CatalogItem>
    {
        public void Configure(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.ToTable("CatalogItems");
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Type).HasConversion<string>();
            builder.HasMany(x => x.Prices)
                .WithOne(x => x.CatalogItem)
                .HasForeignKey(x => x.CatalogItemId);
            builder.HasMany(x => x.Pictures)
                .WithOne(x => x.CatalogItem)
                .HasForeignKey(x => x.CatalogItemId);
        }
    }

    public class CatalogItem: StoreEntity
    {
        public CatalogItemType Type { get; set; }
        public bool Published { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string Sku { get; set; }
        public string Gtin { get; set; }

        #region Inventory

        #endregion

        public ICollection<Price> Prices { get; set; }

        public ICollection<CatalogItemPicture> Pictures { get; set; }
    }
}
