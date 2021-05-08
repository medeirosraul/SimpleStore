using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStore.Core.Entities.Schedules
{
    public class ScheduleAvailableTime
    {
        public DateTime Init { get; set; }

        public int ConsecutiveTime { get; set; }
    }
}
