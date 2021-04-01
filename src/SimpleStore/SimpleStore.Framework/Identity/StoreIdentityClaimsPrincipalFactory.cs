using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SimpleStore.Core.Entities.Identity;
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

        public StoreIdentityClaimsPrincipalFactory(UserManager<StoreIdentity> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
            _userManager = userManager;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(StoreIdentity user)
        {
            // Classbase operations.
            var principal = await base.CreateAsync(user);

            //// Fill subscriptions.
            //await _subscriptionService.FillSubscriptions(user);

            //// Issue Identity.
            //var beAuthIdentity = IdentityIssuer.IssueFor(user);

            //// Add identity
            ////principal.AddIdentity(beAuthIdentity);
            //var identity = (ClaimsIdentity)principal.Identity;
            //identity.AddClaims(beAuthIdentity.Claims);

            return principal;
        }
    }
}
