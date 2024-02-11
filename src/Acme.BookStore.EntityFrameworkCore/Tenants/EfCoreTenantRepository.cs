//using Acme.BookStore.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Migrations;
//using Polly;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Volo.Abp.DependencyInjection;
//using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
//using Volo.Abp.EntityFrameworkCore;
//using Volo.Abp.MultiTenancy;
//using Volo.Abp.TenantManagement;
//using Volo.Abp.TenantManagement.EntityFrameworkCore;
//using ITenantManagementDbContext = Acme.BookStore.EntityFrameworkCore.ITenantManagementDbContext;

//namespace Acme.BookStore.Tenants
//{
//    public class EfCoreTenantRepository : EfCoreRepository<ITenantManagementDbContext, Tenant, Guid>, ITenantRepository
//    {
//        public EfCoreTenantRepository(IDbContextProvider<ITenantManagementDbContext> dbContextProvider)
//            : base(dbContextProvider)
//        {

//        }

//        public virtual async Task<Tenant> FindByNameAsync(
//            string normalizedName,
//            bool includeDetails = true,
//            CancellationToken cancellationToken = default)
//        {
//            return await (await GetDbSetAsync())
//                .IncludeDetails(includeDetails)
//                .OrderBy(t => t.Id)
//                .FirstOrDefaultAsync(t => t.Name == normalizedName, GetCancellationToken(cancellationToken));
//        }

//        [Obsolete("Use FindByNameAsync method.")]
//        public virtual Tenant FindByName(string normalizedName, bool includeDetails = true)
//        {
//            return DbSet
//                .IncludeDetails(includeDetails)
//                .OrderBy(t => t.Id)
//                .FirstOrDefault(t => t.Name == normalizedName);
//        }

//        [Obsolete("Use FindAsync method.")]
//        public virtual Tenant FindById(Guid id, bool includeDetails = true)
//        {
//            return DbSet
//                .IncludeDetails(includeDetails)
//                .OrderBy(t => t.Id)
//                .FirstOrDefault(t => t.Id == id);
//        }

//        public virtual async Task<List<Tenant>> GetListAsync(
//            string sorting = null,
//            int maxResultCount = int.MaxValue,
//            int skipCount = 0,
//            string filter = null,
//            bool includeDetails = false,
//            CancellationToken cancellationToken = default)
//        {
//            return await (await GetDbSetAsync())
//                .IncludeDetails(includeDetails)
//                .WhereIf(
//                    !filter.IsNullOrWhiteSpace(),
//                    u =>
//                        u.Name.Contains(filter)
//                )
//                //.OrderBy(sorting.IsNullOrEmpty() ? nameof(Tenant.Name) : sorting)
//                .PageBy(skipCount, maxResultCount)
//                .ToListAsync(GetCancellationToken(cancellationToken));
//        }

//        public virtual async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
//        {
//            return await (await GetQueryableAsync())
//                .WhereIf(
//                    !filter.IsNullOrWhiteSpace(),
//                    u =>
//                        u.Name.Contains(filter)
//                ).CountAsync(cancellationToken: GetCancellationToken(cancellationToken));
//        }

//        [Obsolete("Use WithDetailsAsync method.")]
//        public override IQueryable<Tenant> WithDetails()
//        {
//            return GetQueryable().IncludeDetails();
//        }

//        public override async Task<IQueryable<Tenant>> WithDetailsAsync()
//        {
//            return (await GetQueryableAsync()).IncludeDetails();
//        }
//    }
//}
