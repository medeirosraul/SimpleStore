using SimpleStore.Core.Entities.Schedules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleStore.Web.Areas.Admin.Models.Schedules
{
    public class ScheduleDayViewModel
    {
        public string Id { get; set; }

        public string ScheduleId { get; set; }

        public DateTime Date { get; set; }

        public ICollection<SchedulePeriodViewModel> Periods { get; set; }

        public ScheduleDayViewModel()
        {

        }

        public ScheduleDayViewModel(Day day)
        {
            FromDay(day);
        }

        public ScheduleDayViewModel FromDay(Day day)
        {
            Id = day.Id;
            ScheduleId = day.ScheduleId;
            Date = day.Date;
            Periods = day.Periods?.Select(s => new SchedulePeriodViewModel(s)).ToList();

            return this;
        }

        public Day ToDay()
        {
            return new Day
            {
                Id = Id,
                ScheduleId = ScheduleId,
                Date = Date,
                Periods = Periods?.Select(s => s.ToPeriod()).ToList()
            };
        }
    }
}
