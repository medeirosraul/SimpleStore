using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleStore.Core.Entities.Storages
{
    public class StorageObjectMap : IEntityTypeConfiguration<StorageObject>
    {
        public void Configure(EntityTypeBuilder<StorageObject> builder)
        {
            builder.ToTable("StorageObjects");
        }
    }

    public class StorageObject: StoreEntity
    {
        public byte[] Bytes { get; set; }
    }
}