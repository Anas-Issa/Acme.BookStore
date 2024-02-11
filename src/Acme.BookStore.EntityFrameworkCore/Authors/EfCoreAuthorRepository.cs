using Acme.BookStore.Books;
using Acme.BookStore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
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

       
        public async Task<Author> GetAuthorWithBooksAsync(Guid id)
        {
            var dbSet = await GetDbSetAsync();
            var author = await dbSet.FirstOrDefaultAsync(a => a.Id == id);
            return author;

        }

        public async Task<IQueryable<Author>> GetListAsync(
      int skipCount,
      int maxResultCount,
      string sorting= "Name",
      AuthorFilter filter = null)
        {

            var dbSet = await GetDbSetAsync();
            var authors = dbSet
                .WhereIf(!filter.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(filter.Name))
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(maxResultCount);

            return authors;
            //var dbSet = await GetDbSetAsync();
            //return await dbSet
            //    .WhereIf(
            //        !filter.Name.IsNullOrWhiteSpace(),
            //        author => author.Name.Contains(filter.Name)
            //        )
            //    //.OrderBy(sorting) // error  in documentation implementation 
            //    .OrderBy(author=>sorting) 
            //    .Skip(skipCount)
            //    .Take(maxResultCount)
            //    .ToListAsync();
        }
      

        public async  Task<int> GetTotalCountAsync(AuthorFilter filter)
        {
            var dbSet = await GetDbSetAsync();
            var authors = dbSet
                .WhereIf(!filter.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(filter.Name))
                ;
            return authors.Count();
            throw new NotImplementedException();
        }
    }
}
