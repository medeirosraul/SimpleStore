using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleStore.Core.Entities.Pictures;

namespace SimpleStore.Core.Entities.CatalogItems
{
    public class CatalogItemPictureMap : IEntityTypeConfiguration<CatalogItemPicture>
    {
        public void Configure(EntityTypeBuilder<CatalogItemPicture> builder)
        {
            builder.ToTable("CatalogItemPictures");
            builder.HasOne(x => x.Picture)
                .WithOne()
                .HasForeignKey<CatalogItemPicture>(x => x.PictureId);
        }
    }
    public class CatalogItemPicture: StoreEntity
    {
        public int Order { get; set; }
        public bool Main { get; set; }
        public string PictureId { get; set; }
        public string CatalogItemId { get; set; }

        // Navigation
        public virtual Picture Picture { get; set; }
        public virtual CatalogItem CatalogItem { get; set; }
    }
}
