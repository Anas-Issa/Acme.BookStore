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
        public MemberAppService(IRepository<Member, Guid> repository,
            IRepository<Book,Guid> bookrepoistory,
            
            IRepository<MemberBook> membeerBookRepository)
        : base(repository)
        { 
                _memberBookRepository = membeerBookRepository;
                _bookRepository = bookrepoistory;
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


                // Save changes to the database
                //await CurrentUnitOfWork.SaveChangesAsync();

                // Construct the response
                var result = new BorrowBooksResultDto
            {
                Success = true,
                Message = "Books borrowed successfully",
                BorrowedBookIds = books.Select(book => book.Id).ToList()
            };

            return result;


            //var result = new List<Guid>();
            //foreach (var item in input.BooksId)
            //{
            //    try
            //    {
            //        var book = new MemberBook { MemberId = input.MemeberId, BookId = item,BorrowingDate=DateTime.Now };
            //   var temp= await _memberBookRepository.InsertAsync(book);



            //    }
            //    catch (Exception ex)
            //    {

            //        throw; 
            //    }
            //    return Ok();
            //};
        }
    }
}
