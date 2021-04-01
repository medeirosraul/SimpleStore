using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleStore.Core.Entities.Storages;

namespace SimpleStore.Core.Entities.Pictures
{
    public class Picture: StoreEntity
    {
        public class PictureMap : IEntityTypeConfiguration<Picture>
        {
            public void Configure(EntityTypeBuilder<Picture> builder)
            {
                builder.ToTable("Pictures");
                builder.Property(x => x.FileName).IsRequired();
                builder.HasOne(x => x.StorageObject)
                    .WithOne()
                    .HasForeignKey<Picture>(x => x.StorageObjectId);
            }
        }

        public string Path { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }
        public long Size { get; set; }
        public string Title { get; set; }

        public string StorageObjectId { get; set; }

        public StorageObject StorageObject { get; set; }
    }
}