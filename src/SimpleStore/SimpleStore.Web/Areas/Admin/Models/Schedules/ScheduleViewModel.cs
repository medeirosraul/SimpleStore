using SimpleStore.Core.Entities.Schedules;
using System.Collections.Generic;
using System.Linq;

namespace SimpleStore.Web.Areas.Admin.Models.Schedules
{
    public class ScheduleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<ScheduleDateViewModel> Dates { get; set; }

        public ScheduleViewModel()
        {

        }

        public ScheduleViewModel(Schedule schedule)
        {
            FromSchedule(schedule);
        }

        public ScheduleViewModel FromSchedule(Schedule schedule)
        {
            Id = schedule.Id;
            Name = schedule.Name;
            Dates = schedule.Dates?.Select(s => new ScheduleDateViewModel(s)).ToList();

            return this;
        }

        public Schedule ToSchedule()
        {
            return new Schedule
            {
                Id = Id,
                Name = Name,
                Dates = Dates?.Select(s => s.ToDate()).ToList()
            };
        }
    }
}
