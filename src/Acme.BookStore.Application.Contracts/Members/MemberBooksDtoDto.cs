using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.BookStore.Members
{
    public class MemberBooksDtoDto
    {
        public Guid  BookId { get; set; }
        public Guid  MemberId{ get; set; }
    }
}
