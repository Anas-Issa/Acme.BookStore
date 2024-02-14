using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace Acme.BookStore.Idnetity
{
    public class MyAbpClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
    {
        private readonly ICurrentUser _currentUser;
        public MyAbpClaimsPrincipalContributor(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
        public async Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
        {
            //var currentTenant = context.ServiceProvider.GetRequiredService<ICurrentTenant>();
            var userEmail = _currentUser.Email;
            if (userEmail== "admin@abp.io")
            {
                //var tenantStore = context.ServiceProvider.GetRequiredService<ITenantStore>();
                //var tenant = await tenantStore.FindAsync(tenantId.Value);

                var claimsIdentity = new ClaimsIdentity();
                claimsIdentity.AddIfNotContains(new Claim("Age","Int"));
                context.ClaimsPrincipal.AddIdentity(claimsIdentity);
            }
        }
    }

}
