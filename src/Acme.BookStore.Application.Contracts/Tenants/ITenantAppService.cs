using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.TenantManagement;

namespace Acme.BookStore.Tenants
{
    public interface ITenantAppService : ICrudAppService<TenantDto, Guid, GetTenantsInput, TenantCreateDto, TenantUpdateDto>
    {
        Task<string> GetDefaultConnectionStringAsync(Guid id);

        Task UpdateDefaultConnectionStringAsync(Guid id, string defaultConnectionString);

        Task DeleteDefaultConnectionStringAsync(Guid id);
    }
}
