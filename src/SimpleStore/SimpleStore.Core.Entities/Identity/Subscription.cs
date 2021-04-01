using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleStore.Core.Entities.Stores;
using SimpleStore.Entities;

namespace SimpleStore.Core.Entities.Identity
{
    public class SubscriptionMap: IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            // builder.HasQueryFilter(x => !x.Deleted);
            builder.ToTable("Subscriptions");

            builder.HasOne(x => x.User)
                .WithMany(x => x.AssignedSubscription)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Owner)
                .WithMany(x => x.OwnedSubscriptions)
                .HasForeignKey(x => x.OwnerId);
            
            builder.HasOne(x => x.Store)
                .WithMany(x => x.Subscriptions)
                .HasForeignKey(x => x.StoreId);
        }
    }

    public class Subscription: Entity
    {
        public string UserId { get; set; }
        public string OwnerId { get; set; }
        public string StoreId { get; set; }

        public virtual StoreIdentity User { get; set; }
        public virtual StoreIdentity Owner { get; set; }
        public virtual Store Store { get; set; }
    }
}
