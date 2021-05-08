using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace SimpleStore.Core.Entities.Schedules
{
    public class DayMap : IEntityTypeConfiguration<ScheduleDate>
    {
        public void Configure(EntityTypeBuilder<ScheduleDate> builder)
        {
            builder.ToTable("ScheduleDates");
            builder.HasMany(x => x.Periods)
                .WithOne()
                .HasForeignKey(x => x.DateId);
        }
    }

    public class ScheduleDate: StoreEntity
    {
        public string ScheduleId { get; set; }

        public DateTime DateTime { get; set; }

        // Navigation
        public virtual ICollection<SchedulePeriod> Periods { get; set; }
    }
}
