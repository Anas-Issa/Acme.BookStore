using Acme.BookStore.Books;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Authors
{
    public  class AuthorDto :EntityDto<Guid>
    {
        public Guid TenantId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string  ShortBio { get; set; }

        public List<BookDto> Books { get; set; }= new List<BookDto>();
    }
}
