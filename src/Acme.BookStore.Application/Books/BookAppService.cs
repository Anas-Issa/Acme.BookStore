using Acme.BookStore.Authors;
using Acme.BookStore.Permissions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Acme.BookStore.Books
{
    public  class BookAppService : 
        CrudAppService<Book,
                       BookDto,
                       Guid,
                       BookPagedAndSortedResultRequestDto,
                       CreateUpdateBookDto>, IBookAppService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IAsyncQueryableExecuter _asyncExecuter;
        private readonly IRepository<Book, Guid> _bookRepository;


        public BookAppService(IAsyncQueryableExecuter asyncExecuter, IRepository<Book, Guid> bookRepository, IAuthorRepository authorRepository) :base(bookRepository)
        {
            _asyncExecuter = asyncExecuter;
            _authorRepository = authorRepository;
            _bookRepository=bookRepository;
            //_bookRepository = bookRepository;
            GetPolicyName = BookStorePermissions.Books.Default;
            GetListPolicyName = BookStorePermissions.Books.Default;
            CreatePolicyName = BookStorePermissions.Books.Create;
            UpdatePolicyName = BookStorePermissions.Books.Edit;
            DeletePolicyName= BookStorePermissions.Books.Delete;
        }

        public override async Task<BookDto> GetAsync(Guid id)
        {
            //Get the IQueryable<Book> from the repository
            var queryable = await _bookRepository.GetQueryableAsync();

            //Prepare a query to join books and authors
            var query = from book in queryable
                        join author in await _authorRepository.GetQueryableAsync() on book.AuthorId equals author.Id
                        where book.Id == id
                        select new { book, author };

            //Execute the query and get the book with author
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Book), id);
            }

            var bookDto = ObjectMapper.Map<Book, BookDto>(queryResult.book);
            bookDto.AuthorName = queryResult.author.Name;
            return bookDto;
        }
        public  override async Task<PagedResultDto<BookDto>> GetListAsync(BookPagedAndSortedResultRequestDto input)
        {


            var filter = ObjectMapper.Map<BookPagedAndSortedResultRequestDto, BookFilter>(input);

            var sorting = (string.IsNullOrEmpty(input.Sorting) ? "Name DESC" : input.Sorting).Replace("ShortName", "Name");

            var queryable = await _bookRepository.GetQueryableAsync();
            //Get the books
            var books = await AsyncExecuter.ToListAsync(
                queryable
                    .WhereIf(!input.Name.IsNullOrEmpty(), x => x.Name.Contains(input.Name)) // apply filtering
                    .WhereIf(!filter.Price.IsNullOrWhiteSpace(), x => x.Price.ToString().Contains(filter.Price))
                    .WhereIf(!filter.PublishDate.IsNullOrWhiteSpace(), x => x.PublishDate.ToString().Contains(filter.PublishDate))
                    .OrderBy(x=>sorting)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
            );

          
            var totalCount = await _bookRepository.GetCountAsync();

          

            var bookDtos = ObjectMapper.Map<List<Book>, List<BookDto>>(books);

            return new PagedResultDto<BookDto>(totalCount, bookDtos);



        }
        //private static string NormalizeSorting(string sorting)
        //{
        //    if (sorting.IsNullOrEmpty())
        //    {
        //        return $"book.{nameof(Book.Name)}";
        //    }

        //    if (sorting.Contains("authorName", StringComparison.OrdinalIgnoreCase))
        //    {
        //        return sorting.Replace(
        //            "authorName",
        //            "author.Name",
        //            StringComparison.OrdinalIgnoreCase
        //        );
        //    }

        //    return $"book.{sorting}";
        //}
        public async Task<ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync()
        {
            var authors = await _authorRepository.GetListAsync();
            var result= new ListResultDto<AuthorLookupDto>(
                ObjectMapper.Map<List<Author>, List<AuthorLookupDto>>(authors));  
            return result;

        }

        public  async Task<AuthorBooksDto> CreateAuthorBooksAsync(CreateAuthorBooksDto input)
        {
            var existingAuthor = await _authorRepository.FindByNameAsync(input.Author.Name);
            var result = new AuthorBooksDto();
            if (existingAuthor != null)
            {
                throw new AuthorAlreadyExistsException(input.Author.Name);
            }
            try
            {
                var author = ObjectMapper.Map<CreateAuthorDto, Author>(input.Author);
                await _authorRepository.InsertAsync(author);
                var authorId = author.Id;

                foreach (var item in input.Books)
                {
                    var book = ObjectMapper.Map<CreateBookAuthorDto, Book>(item);
                    book.AuthorId = authorId;
                    await _bookRepository.InsertAsync(book);
                }
                 result = new AuthorBooksDto
                {
                    AuthorId = authorId,
                    Books = ObjectMapper.Map<List<CreateBookAuthorDto>, List<BookDto>>(input.Books),

                    Name = input.Author.Name,
                };
            }
            catch (Exception ex)
            {

                throw new Exception() ;
            }
            

            
            return   result;

        }

        public async Task DeleteAuthorWithBooks(Guid authorId)
        {
            var existingAuthor = await _authorRepository.GetAsync(authorId);
            if(existingAuthor == null)
            {
                throw new EntityNotFoundException(typeof(Author),authorId);
            }
            await _bookRepository.DeleteAsync(b=>b.AuthorId==authorId);
            await _authorRepository.DeleteAsync(authorId);  

        }


        //public override async Task<BookDto> CreateAsync(CreateUpdateBookDto input)
        //{
        //    var book=ObjectMapper.Map<CreateUpdateBookDto,Book>(input);
            
        //    await _bookRepository.InsertAsync(book);
        //    return ObjectMapper.Map<CreateUpdateBookDto, BookDto>(input);
        //}

        //public Task<PagedResultDto<BookDto>> GetListAsync(BookFilterDto input)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
