using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleStore.Core.Entities.Pictures;

namespace SimpleStore.Core.Entities.Catalog
{
    public class CatalogItemPictureMap : IEntityTypeConfiguration<CatalogProductPicture>
    {
        public void Configure(EntityTypeBuilder<CatalogProductPicture> builder)
        {
            builder.ToTable("CatalogProductPictures");
            builder.HasOne(x => x.Picture)
                .WithOne()
                .HasForeignKey<CatalogProductPicture>(x => x.PictureId);
        }
    }
    public class CatalogProductPicture : StoreEntity
    {
        public int Order { get; set; }
        public bool Main { get; set; }
        public string PictureId { get; set; }
        public string ProductId { get; set; }

        // Navigation
        public virtual Picture Picture { get; set; }
    }
}
