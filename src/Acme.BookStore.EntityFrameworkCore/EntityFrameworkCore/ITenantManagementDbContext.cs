using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;

namespace Acme.BookStore.EntityFrameworkCore
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(AbpTenantManagementDbProperties.ConnectionStringName)]
    public interface ITenantManagementDbContext : IEfCoreDbContext
    {
        DbSet<Tenant> Tenants { get; }

        DbSet<TenantConnectionString> TenantConnectionStrings { get; }
    }
}
