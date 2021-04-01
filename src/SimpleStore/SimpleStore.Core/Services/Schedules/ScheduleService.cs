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
        public ScheduleService(StoreDbContext context, IStoreContext storeContext) : base(context, storeContext)
        {

        }

        public override async Task<Schedule> GetById(string id)
        {
            var query = PrepareQuery()
                .Where(p => p.Id == id)
                .Include(p => p.Days.OrderBy(day => day.Date))
                .ThenInclude(p => p.Periods.OrderBy(period => period.Init));

            var result = await Get(query);
            return result.FirstOrDefault();
        }
    }
}
