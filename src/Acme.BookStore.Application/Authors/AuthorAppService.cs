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

namespace Acme.BookStore.Authors
{
    public  class AuthorAppService: BookStoreAppService, IAuthorAppService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly AuthorManager _authorManager;
        private readonly IStringLocalizer<BookStoreResource>    _localizer;
        private readonly IAsyncQueryableExecuter _asyncExecuter;

        public AuthorAppService(IAsyncQueryableExecuter executer, IAuthorRepository authorRepository, AuthorManager authorManager,
            IStringLocalizer<BookStoreResource> localizer)
        {
            _authorManager = authorManager;
            _authorRepository = authorRepository;
            _localizer = localizer;
            _asyncExecuter = executer;
        }
        [Authorize(BookStorePermissions.Authors.Create)]
        public async Task<AuthorDto> CreateAsync(CreateAuthorDto input)
        {
            var author = await _authorManager.CreateAsync(
                input.Name,
                input.BirthDate,
                input.ShortBio
                );
            await _authorRepository.InsertAsync(author);
            return ObjectMapper.Map<Author,AuthorDto>(author);  
        }
        [Authorize(BookStorePermissions.Authors.Delete)]

        public async Task DeleteAsync(Guid id)
        {
            await _authorRepository.DeleteAsync(id);
        }
        public string getMsg(string msg)
        {
            //var localized = L[msg];
            var localized = _localizer[msg];
            return localized;
        }
        public async Task<AuthorDto> GetAsync(Guid id)
        {
            var author= await _authorRepository.GetAsync(id);
            return ObjectMapper.Map<Author,AuthorDto>(author);
        }
        public async Task<PagedResultDto<AuthorDto>> GetListAsync(AuthorPagedAndSortedResultRequestDto input)
        {
            var filter = ObjectMapper.Map<AuthorPagedAndSortedResultRequestDto, AuthorFilter>(input);

            var sorting = (string.IsNullOrEmpty(input.Sorting) ? "Name DESC" : input.Sorting).Replace("ShortName", "Name");

            var authors = await _authorRepository.GetListwithDetailsAsync(input.SkipCount,input.MaxResultCount,sorting,filter,true);

            //var authors = temp;
        //    await AsyncExecuter.ToListAsync(
        //    temp
                
        //);

            var totalCount = authors.Count();
            var authorDtos = ObjectMapper.Map<List<Author>, List<AuthorDto>>(authors);

            return new PagedResultDto<AuthorDto>(totalCount, authorDtos);
        }
        //public async Task<PagedResultDto<AuthorDto>> GetListAsync(AuthorPagedAndSortedResultRequestDto input)
        //{
        //    var filter = ObjectMapper.Map<AuthorPagedAndSortedResultRequestDto, AuthorFilter>(input);

        //    var sorting = (string.IsNullOrEmpty(input.Sorting) ? "Name DESC" : input.Sorting).Replace("ShortName", "Name");

        //    var temp = await _authorRepository.GetListAsync(includeDetails:true);
        //    var queryable = await _authorRepository.GetQueryableAsync();

        //    var authors= await AsyncExecuter.ToListAsync(queryable
        //        .WhereIf(!filter.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(filter.Name))
        //        .OrderBy(a=>input.Sorting)
        //        .Skip(input.SkipCount)
        //        .Take(input.MaxResultCount)

        //        );
        //    var totalCount =await _authorRepository.CountAsync();
        //    var authorDtos=ObjectMapper.Map<List<Author>,List< AuthorDto >> (authors);

        //    return new PagedResultDto<AuthorDto>(totalCount, authorDtos);

        //}
        [Authorize(BookStorePermissions.Authors.Edit)]
        public async  Task UpdateAsync(Guid id, UpdateAuthorDto input)
        {
            var author = await _authorRepository.GetAsync(id);

            if (author.Name != input.Name)
            {
                await _authorManager.ChangeNameAsync(author, input.Name);
            }

            author.BirthDate = input.BirthDate;
            author.ShortBio = input.ShortBio;

            await _authorRepository.UpdateAsync(author);
        }

        public async Task<AuthorDto> GetAuthorWithBooksAsync(Guid id)
        {
            var author = await _authorRepository.GetAsync(id);
            await _authorRepository.EnsureCollectionLoadedAsync(author,a=>a.Books);
            
            
           return  ObjectMapper.Map<Author, AuthorDto>(author);
        }
    }
}
