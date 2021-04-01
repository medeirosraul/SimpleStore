using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace SimpleStore.Core.Entities.Schedules
{
    public class PeriodMap : IEntityTypeConfiguration<Period>
    {
        public void Configure(EntityTypeBuilder<Period> builder)
        {
            builder.ToTable("SchedulePeriod");
            builder.Property(x => x.Granularity)
                .IsRequired()
                .HasDefaultValue(60); // 1h
            builder.HasMany(x => x.Activities)
                .WithOne()
                .HasForeignKey(x => x.PeriodId);
        }
    }

    public class Period: StoreEntity
    {
        public DateTime Init { get; set; }

        public DateTime End { get; set; }

        public int Granularity { get; set; } // In Minutes

        public int Capacity { get; set; }

        public ICollection<Activity> Activities { get; set; }

        // Navigation
        public string DayId { get; set; }
    }
}