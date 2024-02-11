using Acme.BookStore.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Books
{
    public  interface IBookRepository : IRepository<Book,Guid>
    {
        Task<IQueryable<Book>> GetListAsync(int skipCount, int maxResultCount, string sorting= "Name", BookFilter filter= null); 
        Task<int> GetTotalCountAsync(BookFilter filter);
        //Task  CreateAuthorBooks(Author author, ICollection<Book> books);
    }
}
