using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Schedules;
using SimpleStore.Framework.Contexts;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Schedules
{
    public interface IScheduleActivityService: IStoreBaseService<ScheduleActivity>
    {

    }

    public class ScheduleActivityService : StoreBaseService<ScheduleActivity>, IScheduleActivityService
    {
        public ScheduleActivityService(StoreDbContext context, IStoreContext storeContext) : base(context, storeContext)
        {

        }
    }
}
