﻿using Acme.BookStore.Authors;
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
using Microsoft.AspNetCore.Authorization;

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
        
        //private readonly IRepository<Book,Guid> _bookRepository;


        public BookAppService( IAuthorRepository authorRepository, IRepository<Book, Guid> Repository) :base(Repository)
        {
       
            _authorRepository = authorRepository;
            GetPolicyName = BookStorePermissions.Books.Default;
            GetListPolicyName = BookStorePermissions.Books.Default;
            CreatePolicyName = BookStorePermissions.Books.Create;
            UpdatePolicyName = BookStorePermissions.Books.Edit;
            DeletePolicyName= BookStorePermissions.Books.Delete;
        }


        
        public async Task<ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync()
        {
            var authors = await _authorRepository.GetListAsync();
            var result= new ListResultDto<AuthorLookupDto>(
                ObjectMapper.Map<List<Author>, List<AuthorLookupDto>>(authors));  
            return result;

        }
        [Authorize("BookCreation")]
        public override Task<BookDto> CreateAsync(CreateUpdateBookDto input)
        {
            return base.CreateAsync(input);
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
                    await Repository.InsertAsync(book);
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
            await Repository.DeleteAsync(b=>b.AuthorId==authorId);
            await _authorRepository.DeleteAsync(authorId);  

        }

        public async Task AddTranslationsAsync(Guid id, AddBookTranslationDto input)
        {
            var queryable =await Repository.GetQueryableAsync();

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
            await Repository.UpdateAsync(book);
        }

        protected override async Task<IQueryable<Book>> CreateFilteredQueryAsync(BookPagedAndSortedResultRequestDto input)
        {
         
      
            return (await Repository.WithDetailsAsync())
                .WhereIf(!input.Name.IsNullOrEmpty(), x => x.Name.Contains(input.Name))
                .WhereIf(input.MinPrice.HasValue, x => x.Price >= input.MinPrice)
                .WhereIf(input.MaxPrice.HasValue, x => x.Price <= input.MaxPrice)
                .WhereIf(input.PublishDate.HasValue, x => x.PublishDate.Date == input.PublishDate.Value.Date);
           
        }
        public override  Task<PagedResultDto<BookDto>> GetListAsync(BookPagedAndSortedResultRequestDto input)
        {
            var result= base.GetListAsync(input);
            return result;
          
        }
        protected override IQueryable<Book> ApplySorting(IQueryable<Book> query, BookPagedAndSortedResultRequestDto input)
        {
            return base.ApplySorting(query, input);
        }
    }
}
