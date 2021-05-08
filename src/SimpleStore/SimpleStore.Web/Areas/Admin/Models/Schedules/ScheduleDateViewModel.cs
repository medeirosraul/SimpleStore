using SimpleStore.Core.Entities.Schedules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleStore.Web.Areas.Admin.Models.Schedules
{
    public class ScheduleDateViewModel
    {
        public string Id { get; set; }

        public string ScheduleId { get; set; }

        public DateTime Date { get; set; }

        public ICollection<SchedulePeriodViewModel> Periods { get; set; }

        public ScheduleDateViewModel()
        {

        }

        public ScheduleDateViewModel(ScheduleDate date)
        {
            FromDate(date);
        }

        public ScheduleDateViewModel FromDate(ScheduleDate date)
        {
            Id = date.Id;
            ScheduleId = date.ScheduleId;
            Date = date.DateTime;
            Periods = date.Periods?.Select(s => new SchedulePeriodViewModel(s)).ToList();

            return this;
        }

        public ScheduleDate ToDate()
        {
            return new ScheduleDate
            {
                Id = Id,
                ScheduleId = ScheduleId,
                DateTime = Date,
                Periods = Periods?.Select(s => s.ToPeriod()).ToList()
            };
        }
    }
}
