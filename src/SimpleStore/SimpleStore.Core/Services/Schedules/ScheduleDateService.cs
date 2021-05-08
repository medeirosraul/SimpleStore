using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Schedules;
using SimpleStore.Framework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Schedules
{
    public interface IScheduleDateService: IStoreBaseService<ScheduleDate>
    {
        Task<ICollection<ScheduleDate>> GetBySchedule(string scheduleId);
        Task<ICollection<ScheduleDate>> GetBySchedule(string scheduleId, int year);
        Task<ICollection<ScheduleDate>> GetBySchedule(string scheduleId, int year, int month);
    }

    public class ScheduleDateService : StoreBaseService<ScheduleDate>, IScheduleDateService
    {
        public ScheduleDateService(StoreDbContext context, IStoreContext storeContext) : base(context, storeContext)
        {

        }

        public async Task<ICollection<ScheduleDate>> GetBySchedule(string scheduleId)
        {
            return await GetBySchedule(scheduleId, 0, 0);
        }

        public async Task<ICollection<ScheduleDate>> GetBySchedule(string scheduleId, int year)
        {
            return await GetBySchedule(scheduleId, year, 0);
        }

        public async Task<ICollection<ScheduleDate>> GetBySchedule(string scheduleId, int year, int month)
        {
            if (string.IsNullOrEmpty(scheduleId))
                throw new ArgumentException($"Invalid {nameof(scheduleId)}");

            var query = PrepareQuery();
            query = query.Where(p => p.ScheduleId == scheduleId);

            if (year > 0)
                query = query.Where(p => p.DateTime.Year == year);

            if (month > 0)
                query = query.Where(p => p.DateTime.Month == month);

            return await query.OrderBy(p => p.DateTime).ToListAsync();
        }
    }
}
