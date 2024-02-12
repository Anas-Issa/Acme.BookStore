using Acme.BookStore.MultiLingualObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Books
{
    public  class BookDto : AuditedEntityDto<Guid>,IObjectTranslation
    {
        public  Guid TenantId { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName {  get; set; }
        public string Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }
        public string language { get ; set; }
    }
}
