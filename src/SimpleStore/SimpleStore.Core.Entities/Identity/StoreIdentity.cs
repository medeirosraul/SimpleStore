using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SimpleStore.Core.Entities.Identity
{
    public class StoreIdentity: IdentityUser
    {
        public virtual ICollection<Subscription> OwnedSubscriptions{ get; set; }
        public virtual ICollection<Subscription> AssignedSubscription { get; set; }
    }
}