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

       

        public async Task<IQueryable<Book>> GetListAsync(int skipCount, int maxResultCount, string sorting = "Name", BookFilter filter = null)
        {

            var dbSet = await GetDbSetAsync();
            var books =  dbSet
                .WhereIf(!filter.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(filter.Name))
                .WhereIf(!filter.Price.IsNullOrWhiteSpace(), x => x.Price.ToString().Contains(filter.Price))
                .WhereIf(!filter.PublishDate.IsNullOrWhiteSpace(), x => x.PublishDate.ToString().Contains(filter.PublishDate))
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(maxResultCount);
                
            return books;
        }

       

        public async Task<int> GetTotalCountAsync(BookFilter filter)
        {
            var dbSet = await GetDbSetAsync();
            var books =  dbSet
                .WhereIf(!filter.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(filter.Name))
                .WhereIf(!filter.Price.IsNullOrWhiteSpace(), x => x.Price.ToString().Contains(filter.Price))
                .WhereIf(!filter.PublishDate.IsNullOrWhiteSpace(), x => x.PublishDate.ToString().Contains(filter.PublishDate))
                ;
            return books.Count();
        }

        

       

        

        

       
       
        

       

       

        

        

        
    }
}
