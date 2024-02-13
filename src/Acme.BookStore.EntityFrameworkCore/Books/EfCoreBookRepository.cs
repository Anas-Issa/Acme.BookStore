using Acme.BookStore.Authors;
using Acme.BookStore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
using Volo.Abp.Identity;

namespace Acme.BookStore.Books
{
    public  class EfCoreBookRepository : EfCoreRepository<BookStoreDbContext, Book, Guid>,IBookRepository
    {
        public EfCoreBookRepository(IDbContextProvider<BookStoreDbContext> context):base(context)
        {
            
        }

        public override async Task<IQueryable<Book>> WithDetailsAsync()
        {
            // Uses the extension method defined above
            return (await GetQueryableAsync()).IncludeDetails();
        }

        public override Task<List<Book>> GetListAsync(bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return base.GetListAsync(includeDetails, cancellationToken);
        }
    }
    public static class BooksQueryableExtensions
    {
        public static IQueryable<Book> IncludeDetails(
            this IQueryable<Book> queryable,
            bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                .Include(x => x.Author);

        }
    }
}
