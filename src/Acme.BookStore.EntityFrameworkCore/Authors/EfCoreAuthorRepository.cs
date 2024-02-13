using Acme.BookStore.Books;
using Acme.BookStore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Acme.BookStore.Authors
{
    public class EfCoreAuthorRepository : EfCoreRepository<BookStoreDbContext, Author,Guid>, IAuthorRepository
    {
        public EfCoreAuthorRepository(IDbContextProvider<BookStoreDbContext> context):base(context)
        {
            
        }

        public async Task<Author> FindByNameAsync(string name)
        {
            var dbSet = await GetDbSetAsync();
            return await DbSet.FirstOrDefaultAsync(author => author.Name == name);
        }
        public override async Task<IQueryable<Author>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }
       
    }

    public static class AuthorsQueryableExtensions
    {
        public static IQueryable<Author> IncludeDetails(
            this IQueryable<Author> queryable,
            bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                .Include(x => x.Books);
                
        }
    }
}
