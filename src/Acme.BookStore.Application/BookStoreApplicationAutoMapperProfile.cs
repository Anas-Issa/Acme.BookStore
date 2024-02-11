using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Acme.BookStore.Members;
using AutoMapper;

namespace Acme.BookStore;

public class BookStoreApplicationAutoMapperProfile : Profile
{
    public BookStoreApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
        CreateMap<CreateBookAuthorDto, Book>();
        CreateMap<CreateAuthorBooksDto, BookDto>();

        CreateMap<Author, AuthorDto>();
        CreateMap<Author,AuthorLookupDto>();

        CreateMap<CreateAuthorDto, Author>();
        CreateMap<BookPagedAndSortedResultRequestDto, BookFilter>();
        CreateMap<AuthorPAgedAndSortedResultRequestDto, AuthorFilter>();
        CreateMap<CreateBookAuthorDto, BookDto>();


        CreateMap<CreateUpdateMemberDto, Member>();
        CreateMap <Member, MemberDto>();


    }
}
