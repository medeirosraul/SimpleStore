using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Schedules;
using SimpleStore.Framework.Contexts;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Schedules
{
    public interface IScheduleService: IStoreBaseService<Schedule>
    {

    }

    public class ScheduleService : StoreBaseService<Schedule>, IScheduleService
    {
        private readonly IScheduleDateService _scheduleDateService;

        public ScheduleService(StoreDbContext context, IStoreContext storeContext, IScheduleDateService scheduleDateService) : base(context, storeContext)
        {
            _scheduleDateService = scheduleDateService;
        }

        public override async Task<Schedule> GetById(string id, bool tracking = false)
        {
            var query = PrepareQuery(tracking)
                .Where(p => p.Id == id)
                .Include(p => p.Dates.Where(date => !date.Deleted).OrderBy(day => day.DateTime))
                .ThenInclude(p => p.Periods.OrderBy(period => period.Init));

            var result = await Get(query, tracking);
            return result.FirstOrDefault();
        }
    }
}