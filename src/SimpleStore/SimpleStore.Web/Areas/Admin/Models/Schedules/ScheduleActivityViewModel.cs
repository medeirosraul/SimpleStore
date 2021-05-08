using SimpleStore.Core.Entities.Schedules;
using System.Collections.Generic;
using System.Linq;

namespace SimpleStore.Web.Areas.Admin.Models.Schedules
{
    public class ScheduleActivityViewModel
    {
        public string Id { get; set; }

        public string PeriodId { get; set; }

        public string Name { get; set; }

        public ScheduleActivityViewModel()
        {

        }

        public ScheduleActivityViewModel(ScheduleActivity scheduleActivity)
        {
            FromScheduleActivity(scheduleActivity);
        }

        public ScheduleActivityViewModel FromScheduleActivity(ScheduleActivity scheduleActivity)
        {
            Id = scheduleActivity.Id;
            Name = scheduleActivity.Name;

            return this;
        }

        public ScheduleActivity ToScheduleActivity()
        {
            return null;
        }
    }
}
