using Microsoft.AspNetCore.Authorization.Infrastructure;
using Scriban.Runtime.Accessors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Acme.BookStore.Idnetity
{
    public  class IdentityService :ApplicationService,ITransientDependency

    {
        private readonly ICurrentUser _currentUser;
        private readonly IIdentityClaimTypeRepository _identityClaimTypeRepository;
        public IdentityService(ICurrentUser currntUser,IIdentityClaimTypeRepository identityClaimTypeRepository)
        {
            _currentUser = currntUser;
           _identityClaimTypeRepository = identityClaimTypeRepository;
        }
         public async Task<List<IdentityClaimType>>  GetAllClaims()
        {
            try
            {

              return await _identityClaimTypeRepository.GetListAsync();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
         public async Task<IdentityClaimType> CreateClaimTypeAsunc()
        {
            var claimType = new IdentityClaimType(Guid.NewGuid(), "Age", false,true,"","","", IdentityClaimValueType.Int);

           return await _identityClaimTypeRepository.InsertAsync(claimType);
        }
    }
}
