using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Members
{
   public interface IMemberAppService : ICrudAppService<
       MemberDto,
       Guid,
       PagedAndSortedResultRequestDto,
       CreateUpdateMemberDto
       >
    {
        Task<BorrowBooksResultDto> BorrowBook(BorrowBookDto borrowBooks);

    }
}
