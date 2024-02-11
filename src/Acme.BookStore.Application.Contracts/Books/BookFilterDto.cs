using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Books
{
    public  class BookFilterDto : PagedAndSortedResultRequestDto
    {
     public string Id {get;set;}
    public string Name {get;set;}
    public string PublishDate {get; set;}
    public string Price {get; set;}
    }
}
