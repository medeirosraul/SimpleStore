using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace SimpleStore.Core.Entities.Schedules
{
    public class ScheduleMap : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.ToTable("Schedules");
            builder.Property(x => x.Name)
                .IsRequired();
            builder.HasMany(x => x.Days)
                .WithOne()
                .HasForeignKey(x => x.ScheduleId);
        }
    }

    public class Schedule: StoreEntity
    {
        public string Name { get; set; }
        public ICollection<Day> Days { get; set; }
    }
}