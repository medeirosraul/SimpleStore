using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Schedules;
using SimpleStore.Framework.Contexts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Schedules
{
    public interface ISchedulePeriodService: IStoreBaseService<SchedulePeriod>
    {
        Task<ICollection<SchedulePeriod>> GetByDate(string id);
    }

    public class SchedulePeriodService : StoreBaseService<SchedulePeriod>, ISchedulePeriodService
    {
        public SchedulePeriodService(StoreDbContext context, IStoreContext storeContext, IScheduleDateService scheduleDateService) : base(context, storeContext)
        {
            
        }

        public async Task<ICollection<SchedulePeriod>> GetByDate(string id)
        {
            var query = PrepareQuery();

            query = query.Include(p => p.Activities.Where(x => !x.Deleted));
            query = query.Where(p => p.DateId == id);

            return await query.ToListAsync();
        }
    }
}