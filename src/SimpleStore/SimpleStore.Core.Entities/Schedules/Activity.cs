using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace SimpleStore.Core.Entities.Schedules
{
    public class ActivityMap : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.ToTable("ScheduleActivities");
            builder.Property(x => x.Name)
                .IsRequired();
            builder.Property(x => x.Effort)
                .HasDefaultValue(1);
        }
    }

    public class Activity:StoreEntity
    {
        public string Name { get; set; }
        public DateTime Init { get; set; }

        public int Effort { get; set; } // In Minutes

        // Navigation
        public string PeriodId { get; set; }

    }
}
