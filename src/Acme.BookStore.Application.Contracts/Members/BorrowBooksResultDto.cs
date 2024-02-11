using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.BookStore.Members
{
    public  class BorrowBooksResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<Guid> BorrowedBookIds { get; set; }
    }
}
