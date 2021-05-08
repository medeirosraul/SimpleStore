using SimpleStore.Core.Entities.Schedules;
using System;

namespace SimpleStore.Web.Areas.Admin.Models.Schedules
{
    public class SchedulePeriodViewModel
    {
        public string Id { get; set; }

        public string DayId { get; set; }

        public DateTime Init { get; set; }

        public DateTime End { get; set; }

        public int Granularity { get; set; } // In Minutes

        public SchedulePeriodViewModel()
        {

        }

        public SchedulePeriodViewModel(SchedulePeriod schedule)
        {
            FromPeriod(schedule);
        }

        public SchedulePeriodViewModel FromPeriod(SchedulePeriod period)
        {
            Id = period.Id;
            DayId = period.DateId;
            Init = period.Init;
            End = period.End;
            Granularity = period.Granularity;

            return this;
        }

        public SchedulePeriod ToPeriod()
        {
            return new SchedulePeriod
            {
                Id = Id,
                DateId = DayId,
                Init = Init,
                End = End,
                Granularity = Granularity
            };
        }
    }
}
