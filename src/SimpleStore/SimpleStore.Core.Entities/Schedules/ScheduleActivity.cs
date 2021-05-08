using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace SimpleStore.Core.Entities.Schedules
{
    public class ActivityMap : IEntityTypeConfiguration<ScheduleActivity>
    {
        public void Configure(EntityTypeBuilder<ScheduleActivity> builder)
        {
            builder.ToTable("ScheduleActivities");
            builder.Property(x => x.Name)
                .IsRequired();
            builder.Property(x => x.Duration)
                .HasDefaultValue(1);
        }
    }

    public class ScheduleActivity:StoreEntity
    {
        public string Name { get; set; }

        public DateTime Init { get; set; }

        public int Duration { get; set; }

        public string PeriodId { get; set; }
    }
}
