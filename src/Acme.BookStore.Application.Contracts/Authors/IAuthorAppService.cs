using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Authors
{
    public  interface IAuthorAppService : IApplicationService
    {
        Task<AuthorDto> GetAsync(Guid id);

       Task DeleteAsync(Guid id);

  
    }
}
