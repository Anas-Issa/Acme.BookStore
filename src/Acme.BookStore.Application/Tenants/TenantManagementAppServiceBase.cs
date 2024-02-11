using Acme.BookStore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Tenants
{
    public  class TenantManagementAppServiceBase : ApplicationService
    {
        protected TenantManagementAppServiceBase()
        {
            ObjectMapperContext = typeof(BookStoreApplicationModule);
            LocalizationResource = typeof(BookStoreResource);
        }
    }
}
