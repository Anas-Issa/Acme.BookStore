using Acme.BookStore.Books;
using Acme.BookStore.Localization;
using Acme.BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;
using Volo.Abp.ObjectMapping;
using System.Linq.Dynamic.Core;
using Volo.Abp.Auditing;
using Volo.Abp;

namespace Acme.BookStore.Authors
{
    public  class AuthorAppService: BookStoreAppService, IAuthorAppService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;   
        private readonly AuthorManager _authorManager;
        private readonly IStringLocalizer<BookStoreResource>    _localizer;
        //private readonly IAsyncQueryableExecuter _asyncExecuter;

        public AuthorAppService(IAuthorRepository authorRepository, 
            IStringLocalizer<BookStoreResource> localizer,
            AuthorManager authorManager
,
            IBookRepository bookRepository)
        {
            _authorRepository = authorRepository;
            _localizer = localizer;
            _authorManager = authorManager;
            _bookRepository = bookRepository;
        }
        [Authorize(BookStorePermissions.Authors.Create)]
        public async Task<AuthorBooksDto> CreateAsync(CreateAuthorBooksDto input)
        {
            //var author = await _authorManager.CreateAsync(
            //    input.Name,
            //    input.BirthDate,
            //    input.ShortBio
            //    );
            //await _authorRepository.InsertAsync(author);
            //return ObjectMapper.Map<Author,AuthorDto>(author);  
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

                throw new Exception();
            }



            return result;
        }
        [Authorize(BookStorePermissions.Authors.Delete)]

        public async Task DeleteAsync(Guid id)
        {
            await _authorRepository.DeleteAsync(id);
        }
       
        public async Task<AuthorDto> GetAsync(Guid id)
        {
            var author= await _authorRepository.GetAsync(id);
            return ObjectMapper.Map<Author,AuthorDto>(author);
        }
    

        private async Task<IQueryable<Author>> CreateFilteredQueryAsync(AuthorPagedAndSortedResultRequestDto input)
        {
            return (await _authorRepository.WithDetailsAsync())
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), a => a.Name.Contains(input.Name));
        }
      public  async Task<PagedResultDto<AuthorDto>> GetListAsync(AuthorPagedAndSortedResultRequestDto input)
        {
            //await CheckGetListPolicyAsync();

            var query = await CreateFilteredQueryAsync(input);
            var totalCount = await AsyncExecuter.CountAsync(query);

            var entities = new List<Author>();
            var entityDtos = new List<AuthorDto>();

            if (totalCount > 0)
            {
                query = ApplySorting(query, input);
                query = ApplyPaging(query, input);

                entities = await AsyncExecuter.ToListAsync(query);
                entityDtos = ObjectMapper.Map<List<Author>, List<AuthorDto>>(entities);
            }

            return new PagedResultDto<AuthorDto>(
                totalCount,
                entityDtos
            );
        }
        private IQueryable<Author> ApplySorting(IQueryable<Author> query, AuthorPagedAndSortedResultRequestDto input)
        {
            if (input is ISortedResultRequest sortInput)
            {
                if (!sortInput.Sorting.IsNullOrWhiteSpace())
                {
                    return query.OrderBy(sortInput.Sorting!);
                }
            }

            //IQueryable.Task requires sorting, so we should sort if Take will be used.
            if (input is ILimitedResultRequest)
            {
                return ApplyDefaultSorting(query);
            }

            //No sorting
            return query;
        }
      private  IQueryable<Author> ApplyDefaultSorting(IQueryable<Author> query)
        {
            if (typeof(Author).IsAssignableTo<IHasCreationTime>())
            {
                return query.OrderByDescending(e => ((IHasCreationTime)e).CreationTime);
            }

            throw new AbpException("No sorting specified but this query requires sorting. Override the ApplySorting or the ApplyDefaultSorting method for your application service derived from AbstractKeyReadOnlyAppService!");
        }
       private IQueryable<Author> ApplyPaging(IQueryable<Author> query, AuthorPagedAndSortedResultRequestDto input)
        {
            //Try to use paging if available
            if (input is IPagedResultRequest pagedInput)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            if (input is ILimitedResultRequest limitedInput)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }
    
        [Authorize(BookStorePermissions.Authors.Edit)]
        public async Task<AuthorBooksDto> UpdateAsync(Guid id, CreateAuthorBooksDto input)
        {
            var result = new AuthorBooksDto();
            var existingAuthor = await _authorRepository.GetAsync(id);

            if (existingAuthor.Name != input.Author.Name)
            {
                await _authorManager.ChangeNameAsync(existingAuthor, input.Author.Name);
            }

            // Remove books that are not in the update request
            var booksToRemove = existingAuthor.Books.Where(book => !input.Books.Any(b => b.Id == book.Id)).ToList();
            var booksToUpdate = existingAuthor.Books.Where(book => input.Books.Any(b => b.Id == book.Id)).ToList();

            foreach (var book in booksToRemove)
            {
                // Detach the book entity from the context before deletion
                //_bookRepository.Detach(book);
                await _bookRepository.DeleteAsync(book.Id);
            }

            foreach (var bookDto in input.Books)
            {
                var existingBook = existingAuthor.Books.FirstOrDefault(book => book.Id == bookDto.Id);

                if (existingBook != null)
                {
                    // Update existing book
                    ObjectMapper.Map(bookDto, existingBook);
                    await _bookRepository.UpdateAsync(existingBook);
                }
                else
                {
                    // Add new book
                    var newBook = ObjectMapper.Map<CreateUpdateBookDto, Book>(bookDto);
                    newBook.AuthorId = existingAuthor.Id;
                    await _bookRepository.InsertAsync(newBook);
                }
            }

            // Update author properties
            existingAuthor.BirthDate = input.Author.BirthDate;
            existingAuthor.ShortBio = input.Author.ShortBio;

            await _authorRepository.UpdateAsync(existingAuthor);

            result = new AuthorBooksDto
            {
                Books = ObjectMapper.Map<List<CreateUpdateBookDto>, List<BookDto>>(input.Books),
                Name = input.Author.Name,
            };

            return result;
            
        }

       
    }
}
