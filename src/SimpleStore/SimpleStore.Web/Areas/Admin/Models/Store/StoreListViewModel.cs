using SimpleStore.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Admin.Models.Store
{
    public class StoreListViewModel
    {
        public IEnumerable<SubscriptionViewModel> Subscriptions { get; set; }
    }
}
