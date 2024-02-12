using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Books
{
    public  class BookPagedAndSortedResultRequestDto:PagedAndSortedResultRequestDto
    {
        public string? Name { get; set; }
        public string? PublishDate { get; set; }
        public string? Price { get; set; }
        //public bool? IsAscending { get; set; }  
    } 
}
