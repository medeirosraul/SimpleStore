using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Identity;
using SimpleStore.Core.Services.Stores;
using SimpleStore.Framework.Repositories;
using SimpleStore.Framework.Types;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Subscriptions
{
    public class SubscriptionService: BaseService<Subscription>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubscriptionService(StoreDbContext context, IHttpContextAccessor httpContext): base(context)
        {
            _httpContextAccessor = httpContext;
        }

        public override Task<PagedList<Subscription>> Get(IQueryable<Subscription> query, bool tracking = false)
        {
            // User data
            var identity = _httpContextAccessor.HttpContext.User;
            var userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            query = PrepareQuery(tracking);
            query = query.Where(p => p.UserId == userId);
            query = query.Include(p => p.Store);
            return base.Get(query);
        }

        public Task<PagedList<Subscription>> GetByUser(string userId)
        {
            var query = PrepareQuery();
            query = query.Where(p => p.UserId == userId);
            return base.Get(query);
        }
    }
}