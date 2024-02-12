using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Authors
{
    public  interface IAuthorRepository : IRepository<Author,Guid>
    {
        Task<Author> FindByNameAsync(string name);

        Task<List<Author>> GetListwithDetailsAsync(int skipCount, int maxResultCount, string sorting, AuthorFilter filter = null );

        //Task<int> GetTotalCountAsync(AuthorFilter filter);
        //Task<Author> GetAuthorWithBooksAsync(Guid id);
        
    }
}
