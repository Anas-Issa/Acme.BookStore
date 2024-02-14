using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.BookStore.Members
{
    public class MemberBooksDto
    {
        public Guid  BookId { get; set; }
        public Guid  MemberId{ get; set; }
        public string  BookName { get; set; }
    }
}
