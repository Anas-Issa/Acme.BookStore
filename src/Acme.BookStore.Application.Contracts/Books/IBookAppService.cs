using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Books
{
    public interface IBookAppService :
        ICrudAppService <BookDto,
            Guid,
            BookPagedAndSortedResultRequestDto,
            CreateUpdateBookDto>
    {
        Task <ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync();

        Task<AuthorBooksDto> CreateAuthorBooksAsync(CreateAuthorBooksDto input);

        Task AddTranslationsAsync (Guid id,AddBookTranslationDto input);
    }
}
