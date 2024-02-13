using Acme.BookStore.Authors;
using Acme.BookStore.Members;
using Acme.BookStore.MultiLingualObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Acme.BookStore.Books
{
    public  class Book : FullAuditedAggregateRoot<Guid>,IMultiTenant,IMultiLingualObjects<BookTranslation>
    {
        public Book()
        {
            Translations= new List<BookTranslation>();
        }
        public Guid? TenantId { get; set; }

        
        public Guid AuthorId { get; set; }  
        public string Name { get; set; }
        public BookType Type { get; set; }
        public DateTime PublishDate { get; set; }
        public float  Price { get; set; }

         public virtual Author Author { get; set; }
        public virtual  ICollection<MemberBook> Borrowers { get; set; }
        public  ICollection<BookTranslation> Translations { get ; set; } 
    }
}
