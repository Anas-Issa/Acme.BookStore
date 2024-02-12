using Acme.BookStore.MultiLingualObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Acme.BookStore.Books
{
    public  class BookTranslation:Entity, IObjectTranslation
    {
        public Guid BookId { get; set; }
        public string Name { get; set; }
        public string language { get; set ; }

        public override object[] GetKeys()
        {
            return new object[] { BookId, language };
        }
    }
}
