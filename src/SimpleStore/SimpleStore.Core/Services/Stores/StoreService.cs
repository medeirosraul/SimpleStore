using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Identity;
using SimpleStore.Core.Entities.Stores;
using SimpleStore.Core.Services.Subscriptions;
using SimpleStore.Framework.Types;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Stores
{
    public class StoreService: BaseService<Store>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SubscriptionService _subscriptionService;

        public StoreService(StoreDbContext context, IHttpContextAccessor httpContextAcessor, SubscriptionService subscriptionService) : base(context)
        {
            _httpContextAccessor = httpContextAcessor;
            _subscriptionService = subscriptionService;
        }

        public async Task<Store> CreateStore(Store store)
        {
            // User data
            var identity = _httpContextAccessor.HttpContext.User;
            var userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Create store
            await InsertOrUpdate(store);
            if (store == null) throw new InvalidOperationException();

            // Create subscription
            var subscription = new Subscription
            {
                UserId = userId,
                OwnerId = userId,
                StoreId = store.Id
            };

            await _subscriptionService.InsertOrUpdate(subscription);
            store.Subscriptions.Add(subscription);
            return store;
        }
        public override IQueryable<Store> PrepareQuery(bool tracking = false, bool deleted = false)
        {
            var query = base.PrepareQuery(tracking, deleted);
            query = query.Include(x => x.LogoPicture);

            return query;
        }
        public async Task<Store> GetStoreBySubdomain(string subdomain)
        {
            return await PrepareQuery()
                .Where(p => p.Subdomain == subdomain)
                .FirstOrDefaultAsync();
        }

        public async Task<Store> GetStoreByHost(string host)
        {
            return await PrepareQuery()
                .Where(p => p.Host == host)
                .FirstOrDefaultAsync();
        }

        public override async Task<PagedList<Store>> Get(bool tracking = false)
        {
            return await base.Get(tracking);
        }
    }
}