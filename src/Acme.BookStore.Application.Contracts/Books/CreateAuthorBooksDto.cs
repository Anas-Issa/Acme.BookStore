using Acme.BookStore.Authors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.BookStore.Books
{
    public  class CreateAuthorBooksDto
    {
        public CreateAuthorDto Author { get; set; }
        public List<CreateBookAuthorDto> Books { get; set; } = new List<CreateBookAuthorDto>();
    }
}
