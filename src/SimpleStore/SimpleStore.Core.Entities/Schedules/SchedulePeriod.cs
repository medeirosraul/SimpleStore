using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleStore.Core.Entities.Schedules
{
    public class PeriodMap : IEntityTypeConfiguration<SchedulePeriod>
    {
        public void Configure(EntityTypeBuilder<SchedulePeriod> builder)
        {
            builder.ToTable("SchedulePeriods");
            builder.Property(x => x.Granularity)
                .IsRequired()
                .HasDefaultValue(60); // 1h
            builder.HasMany(x => x.Activities)
                .WithOne()
                .HasForeignKey(x => x.PeriodId);
        }
    }

    public class SchedulePeriod: StoreEntity
    {
        public string DateId { get; set; }

        public DateTime Init { get; set; }

        public DateTime End { get; set; }

        public int Granularity { get; set; } // In Minutes

        // Navigation
        public virtual ICollection<ScheduleActivity> Activities { get; set; }

        // Not mapped
        [NotMapped]
        public ICollection<ScheduleAvailableTime> AvailableTimes { get; set; }
    }
}