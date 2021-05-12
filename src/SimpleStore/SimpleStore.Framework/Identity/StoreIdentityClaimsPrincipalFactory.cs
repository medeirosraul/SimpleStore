using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SimpleStore.Core.Entities.Identity;
using SimpleStore.Framework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStore.Framework.Identity
{
    public class StoreIdentityClaimsPrincipalFactory : UserClaimsPrincipalFactory<StoreIdentity>
    {
        private readonly UserManager<StoreIdentity> _userManager;
        private readonly IStoreContext _storeContext;

        public StoreIdentityClaimsPrincipalFactory(UserManager<StoreIdentity> userManager, IOptions<IdentityOptions> optionsAccessor, IStoreContext storeContext) : base(userManager, optionsAccessor)
        {
            _userManager = userManager;
            _storeContext = storeContext;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(StoreIdentity user)
        {
            // Get user with subscriptions
            var completeUser = await UserManager.Users.Where(p => p.Id == user.Id)
                .Include(x => x.OwnedSubscriptions)
                .SingleAsync();

            // Create principal
            var principal = await base.CreateAsync(user);

            // If SimpleStore main site, return
            if (_storeContext.IsSimpleStore())
                return principal;

            // If don't have subscriptions, return
            if (completeUser.OwnedSubscriptions == null || completeUser.OwnedSubscriptions.Count == 0)
                return principal;

            // Create claim if store owner
            var identity = (ClaimsIdentity)principal.Identity;
            foreach(var sub in completeUser.OwnedSubscriptions)
            {
                if(sub.StoreId == _storeContext.CurrentStore.Id)
                    identity.AddClaim(new Claim("StoreOwner", sub.StoreId));
            }

            return principal;
        }
    }
}
