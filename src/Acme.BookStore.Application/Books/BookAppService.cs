using Acme.BookStore.Authors;
using Acme.BookStore.Permissions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;
using System.Linq.Dynamic.Core;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Acme.BookStore.Books
{
    /// <summary>
    /// Represent a Book Entity
    /// </summary>
    public  class BookAppService : 
        CrudAppService<Book,
                       BookDto,
                       Guid,
                       BookPagedAndSortedResultRequestDto,
                       CreateUpdateBookDto>, IBookAppService
    {
        private readonly IAuthorRepository _authorRepository;
        
        private readonly IRepository<Book, Guid> _bookRepository;


        public BookAppService( IRepository<Book, Guid> bookRepository, IAuthorRepository authorRepository) :base(bookRepository)
        {
       
            _authorRepository = authorRepository;
            _bookRepository=bookRepository;
            //_bookRepository = bookRepository;
            GetPolicyName = BookStorePermissions.Books.Default;
            GetListPolicyName = BookStorePermissions.Books.Default;
            CreatePolicyName = BookStorePermissions.Books.Create;
            UpdatePolicyName = BookStorePermissions.Books.Edit;
            DeletePolicyName= BookStorePermissions.Books.Delete;
        }
        /// <summary>
        /// Get book with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
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
                    var book = ObjectMapper.Map<CreateUpdateBookDto, Book>(item);
                    book.AuthorId = authorId;
                    await _bookRepository.InsertAsync(book);
                }
                 result = new AuthorBooksDto
                {
                    AuthorId = authorId,
                    Books = ObjectMapper.Map<List<CreateUpdateBookDto>, List<BookDto>>(input.Books),

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

        public async Task AddTranslationsAsync(Guid id, AddBookTranslationDto input)
        {
            var queryable =await Repository.GetQueryableAsync();
           // var bookquery = queryable.FirstOrDefault(b=>b.Id==id);

            var book = await AsyncExecuter.FirstOrDefaultAsync(queryable, x => x.Id == id);

            if ( book.Translations!= null &&  book.Translations.Any(b => b.language == input.Language)  )
            {
                throw new UserFriendlyException($"Thers is already translation in {input.Language}");
            }
            book.Translations.Add(new BookTranslation
            {
                BookId = id,
                language = input.Language,
                 Name=input.Name
            });
            await _bookRepository.UpdateAsync(book);
        }

        protected override async Task<IQueryable<Book>> CreateFilteredQueryAsync(BookPagedAndSortedResultRequestDto input)
        {
            var query = (await base.CreateFilteredQueryAsync(input))
                .WhereIf(!input.Name.IsNullOrEmpty(), x => x.Name.Contains(input.Name))
                .WhereIf(input.MinPrice.HasValue, x => x.Price >= input.MinPrice)
                .WhereIf(input.MaxPrice.HasValue, x => x.Price <= input.MaxPrice)
                .WhereIf(input.PublishDate.HasValue, x => x.PublishDate.Date == input.PublishDate.Value.Date);

            return query;
        }

        protected override IQueryable<Book> ApplySorting(IQueryable<Book> query, BookPagedAndSortedResultRequestDto input)
        {
            return base.ApplySorting(query, input);
        }
    }
}
