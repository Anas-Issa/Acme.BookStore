//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Volo.Abp.TenantManagement;
//using Volo.Abp;
//using Volo.Abp.EntityFrameworkCore.Modeling;

//namespace Acme.BookStore.EntityFrameworkCore
//{
//    public static class AbpTenantManagementDbContextModelCreatingExtensions
//    {
//        public static void ConfigureTenantManagement(
//            this ModelBuilder builder)
//        {
//            Check.NotNull(builder, nameof(builder));

//            if (builder.IsTenantOnlyDatabase())
//            {
//                return;
//            }

//            builder.Entity<Tenant>(b =>
//            {
//                b.ToTable(AbpTenantManagementDbProperties.DbTablePrefix + "Tenants", AbpTenantManagementDbProperties.DbSchema);

//                b.ConfigureByConvention();

//                b.Property(t => t.Name).IsRequired().HasMaxLength(TenantConsts.MaxNameLength);
//                b.Property(t => t.Name).IsRequired().HasMaxLength(TenantConsts.MaxNameLength);

//                b.HasMany(u => u.ConnectionStrings).WithOne().HasForeignKey(uc => uc.TenantId).IsRequired();

//                b.HasIndex(u => u.Name);
//                b.HasIndex(u => u.Name);

//                b.ApplyObjectExtensionMappings();
//            });

//            builder.Entity<TenantConnectionString>(b =>
//            {
//                b.ToTable(AbpTenantManagementDbProperties.DbTablePrefix + "TenantConnectionStrings", AbpTenantManagementDbProperties.DbSchema);

//                b.ConfigureByConvention();

//                b.HasKey(x => new { x.TenantId, x.Name });

//                b.Property(cs => cs.Name).IsRequired().HasMaxLength(TenantConnectionStringConsts.MaxNameLength);
//                b.Property(cs => cs.Value).IsRequired().HasMaxLength(TenantConnectionStringConsts.MaxValueLength);

//                b.ApplyObjectExtensionMappings();
//            });

//            builder.TryConfigureObjectExtensions<TenantManagementDbContext>();
//        }
//    }
//}
