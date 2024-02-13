using Acme.BookStore.Authors;
using Acme.BookStore.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Acme.BookStore.Books
{
    public  class EfCoreBookRepository : EfCoreRepository<BookStoreDbContext, Book, Guid>,IBookRepository
    {
        public EfCoreBookRepository(IDbContextProvider<BookStoreDbContext> context):base(context)
        {
            
        }

       
    }
}
