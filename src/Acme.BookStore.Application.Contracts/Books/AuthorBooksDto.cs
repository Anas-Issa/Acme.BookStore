using Acme.BookStore.Authors;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Books
{
    public  class AuthorBooksDto 
    {
        public Guid  AuthorId { get; set; }
        public  string  Name { get; set; }
        public List<BookDto> Books { get; set; } = new List<BookDto>();
    }
}
