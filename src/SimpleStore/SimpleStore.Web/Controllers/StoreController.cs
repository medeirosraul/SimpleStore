using Microsoft.AspNetCore.Mvc;
using SimpleStore.Core.Entities.Identity;
using SimpleStore.Core.Entities.Stores;
using SimpleStore.Core.Services.Stores;
using SimpleStore.Core.Services.Subscriptions;
using SimpleStore.Framework.Contexts;
using SimpleStore.Web.Areas.Admin.Models.Store;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Web.Controllers
{
    public class StoreController : Controller
    {
        private readonly IStoreContext _storeContext;
        private readonly StoreService _storeService;
        private readonly SubscriptionService _subscriptionService;

        public StoreController(IStoreContext storeContext, StoreService storeService, SubscriptionService subscriptionService)
        {
            _storeContext = storeContext;
            _storeService = storeService;
            _subscriptionService = subscriptionService;
        }

        public async Task<IActionResult> Index()
        {
            // Get all subscriptions for this account to show Stores to user 
            // for selection
            var subscriptions = await _subscriptionService.Get(null);
            if (subscriptions == null || subscriptions.Count == 0) return View();

            var list = subscriptions.Select(s =>
            {
                return new SubscriptionViewModel
                {
                    Store = new StoreViewModel
                    {
                        Id = s.Store.Id,
                        Name = s.Store.Name,
                        Subdomain = s.Store.Subdomain
                    }
                };
            });

            var result = new StoreListViewModel
            {
                Subscriptions = list
            };

            return View(result);
        }

        public IActionResult Create()
        {
            return View(new StoreViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(StoreViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var entity = new Store
            {
                Name = model.Name,
                Subdomain = model.Subdomain
            };

            await _storeService.CreateStore(entity);
            return View(model);
        }
    }
}
