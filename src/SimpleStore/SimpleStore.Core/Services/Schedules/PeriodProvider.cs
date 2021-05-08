using SimpleStore.Core.Entities.Schedules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Schedules
{
    public interface IPeriodProvider
    {
        ICollection<DateTime> CalculateAvailableTimes(DateTime init, DateTime end, int granularity);
        ICollection<ScheduleAvailableTime> CalculateAvailableTimes(SchedulePeriod period);
        ICollection<ScheduleAvailableTime> CalculateConsecutiveTimes(SchedulePeriod period);
        SchedulePeriod CalculatePeriod(SchedulePeriod period);
        Task<ICollection<SchedulePeriod>> GetCalculatedPeriods(ScheduleDate date);
    }

    public class PeriodProvider : IPeriodProvider
    {
        private readonly ISchedulePeriodService _periodService;

        public PeriodProvider(ISchedulePeriodService periodService)
        {
            _periodService = periodService;
        }

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

        public ICollection<ScheduleAvailableTime> CalculateAvailableTimes(SchedulePeriod period)
        {
            // Get times
            var times = CalculateAvailableTimes(period.Init, period.End, period.Granularity);
            period.AvailableTimes = new List<ScheduleAvailableTime>();

            // Create available times
            foreach(var time in times)
            {
                period.AvailableTimes.Add(new ScheduleAvailableTime
                {
                    Init = time
                });
            }

            // Order times
            period.AvailableTimes = period.AvailableTimes.OrderBy(x => x.Init).ToList();

            // Return if no activities
            if (period.Activities == null || period.Activities.Count == 0) 
                return period.AvailableTimes;

            // Remove alocated times
            foreach(var activity in period.Activities)
            {
                var activityInit = activity.Init;
                var activityEnd = activityInit.AddMinutes(activity.Duration);

                var allocatedTimes = period.AvailableTimes.Where(p => p.Init >= activityInit && p.Init < activityEnd);

                foreach (var time in allocatedTimes)
                    period.AvailableTimes.Remove(time);
            }

            // Return
            return period.AvailableTimes;
        }

        public ICollection<ScheduleAvailableTime> CalculateConsecutiveTimes(SchedulePeriod period)
        {
            foreach(var time in period.AvailableTimes)
            {
                var nextTimeExists = true;
                while (nextTimeExists)
                {
                    time.ConsecutiveTime += period.Granularity;
                    if (!period.AvailableTimes.Any(p => p.Init == time.Init.AddMinutes(time.ConsecutiveTime)))
                        nextTimeExists = false;
                }
            }

            return period.AvailableTimes;
        }

        public SchedulePeriod CalculatePeriod(SchedulePeriod period)
        {
            CalculateAvailableTimes(period);
            CalculateConsecutiveTimes(period);
            return period;
        }

        public async Task<ICollection<SchedulePeriod>> GetCalculatedPeriods(ScheduleDate date)
        {
            var periods = await _periodService.GetByDate(date.Id);
            foreach(var period in periods)
            {
                CalculatePeriod(period);
            }

            return periods;
        }
    }
}
