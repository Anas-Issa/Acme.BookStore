using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.BookStore.Members
{
    public  class BorrowBookDto
    {
        public Guid MemeberId { get; set; }
        public List<Guid> BooksId { get; set; }
    }
}
