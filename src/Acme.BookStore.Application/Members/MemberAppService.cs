using Acme.BookStore.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Members
{
    public  class MemberAppService :CrudAppService<Member,
        MemberDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateMemberDto>,IMemberAppService
    {
        private readonly IRepository<MemberBook> _memberBookRepository;
        private readonly IRepository<Book,Guid> _bookRepository;
        //private readonly IMemberRepository _memberRepository;
        public MemberAppService(IMemberRepository repository,
            IRepository<Book,Guid> bookrepoistory,
            
            IRepository<MemberBook> memberBookRepository
            //IMemberRepository memberRepository
            )
        : base(repository)
        { 
                _memberBookRepository = memberBookRepository;
                _bookRepository = bookrepoistory;
            //_memberRepository = memberRepository;
        }
        
        public async Task<BorrowBooksResultDto> BorrowBook(BorrowBookDto input)
        {
            var member = await Repository.GetAsync(input.MemeberId);

            if (member == null)
            {
                throw new EntityNotFoundException(typeof(Member), input.MemeberId);
            }

            var books = await _bookRepository.GetListAsync(book => input.BooksId.Contains(book.Id));

            // Implement the logic to update entities for borrowing the books
            // For example, add the books to the member's BorrowedBooks collection
            foreach (var item in input.BooksId)
            {
                var book = new MemberBook { MemberId = input.MemeberId, BookId = item, BorrowingDate = DateTime.Now };
                var temp = await _memberBookRepository.InsertAsync(book);
             }
                // Construct the response
                var result = new BorrowBooksResultDto
            {
                Success = true,
                Message = "Books borrowed successfully",
                BorrowedBookIds = books.Select(book => book.Id).ToList()
            };
            return result;         
        }
        protected override async Task<IQueryable<Member>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
        {
            return await Repository.WithDetailsAsync();
            //return base.CreateFilteredQueryAsync(input);
        }


    }
}
