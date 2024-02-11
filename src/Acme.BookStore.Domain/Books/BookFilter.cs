using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BookStore.Books
{
    public  class BookFilter
    {
        public string? Name { get; set; }
        public string? PublishDate { get; set; }
        public string? Price { get; set; }
    }
}
