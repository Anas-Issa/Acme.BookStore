using Acme.BookStore.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Acme.BookStore.EntityFrameworkCore.Applications.Books
{
    [Collection(BookStoreTestConsts.CollectionDefinitionName)]
    public  class EfCoreBookAppService_Test :BookAppService_Tests<BookStoreEntityFrameworkCoreTestModule>
    {
    }
}
