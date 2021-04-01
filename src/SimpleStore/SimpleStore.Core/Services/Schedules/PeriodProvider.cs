using SimpleStore.Core.Entities.Schedules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Schedules
{
    public interface IPeriodProvider
    {
        ICollection<DateTime> CalculateAvailableTimes(DateTime init, DateTime end, int granularity);
        ICollection<DateTime> CalculateAvailableTimes(Period period);
        ICollection<DateTime> CalculateAvailableTimes(ICollection<Period> periods);
    }

    public class PeriodProvider : IPeriodProvider
    {
        public ICollection<DateTime> CalculateAvailableTimes(DateTime init, DateTime end, int granularity)
        {
            var result = new List<DateTime>();

            var availableTime = init;
            while (availableTime.AddMinutes(granularity) <= end)
            {
                result.Add(availableTime);
                availableTime = availableTime.AddMinutes(granularity);
            }

            return result;
        }

        public ICollection<DateTime> CalculateAvailableTimes(Period period)
        {
            return CalculateAvailableTimes(period.Init, period.End, period.Granularity);
        }

        public ICollection<DateTime> CalculateAvailableTimes(ICollection<Period> periods)
        {
            var result = new List<DateTime>();

            foreach(var period in periods)
            {
                result.AddRange(CalculateAvailableTimes(period));
            }

            return result.OrderBy(p => p.Date).ToList();
        }
    }
}
