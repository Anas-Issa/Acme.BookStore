//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Volo.Abp.Data;
//using Volo.Abp.EntityFrameworkCore;
//using Volo.Abp.MultiTenancy;
//using Volo.Abp.TenantManagement;

//namespace Acme.BookStore.EntityFrameworkCore
//{
//    [IgnoreMultiTenancy]
//    [ConnectionStringName(AbpTenantManagementDbProperties.ConnectionStringName)]
//    public class TenantManagementDbContext : AbpDbContext<TenantManagementDbContext>, ITenantManagementDbContext
//    {
//        public DbSet<Tenant> Tenants { get; set; }

//        public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

//        public TenantManagementDbContext(DbContextOptions<TenantManagementDbContext> options)
//            : base(options)
//        {
//        }

//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            base.OnModelCreating(builder);

//            builder.ConfigureTenantManagement();
//        }
//    }
//}