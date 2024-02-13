using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Books
{
    public  class BookPagedAndSortedResultRequestDto:PagedAndSortedResultRequestDto
    {
        public string? Name { get; set; }
        public DateTime? PublishDate { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
        //public bool? IsAscending { get; set; }  
    } 
}
