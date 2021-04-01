using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace SimpleStore.Core.Entities.Schedules
{
    public class DayMap : IEntityTypeConfiguration<Day>
    {
        public void Configure(EntityTypeBuilder<Day> builder)
        {
            builder.ToTable("ScheduleDays");
            builder.HasMany(x => x.Periods)
                .WithOne()
                .HasForeignKey(x => x.DayId);
        }
    }

    public class Day: StoreEntity
    {

        public DateTime Date { get; set; }

        public ICollection<Period> Periods { get; set; }

        // Navigation
        public string ScheduleId { get; set; }
    }
}
