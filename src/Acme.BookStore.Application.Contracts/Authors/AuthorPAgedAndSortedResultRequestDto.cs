using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Authors
{
    public  class AuthorPagedAndSortedResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? Name { get; set; }
    }
}
