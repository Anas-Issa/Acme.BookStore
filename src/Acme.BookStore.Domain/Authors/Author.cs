using Acme.BookStore.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Acme.BookStore.Authors
{
    public  class Author : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public string  Name { get; private set; }
        public DateTime BirthDate { get; set; }
        public string ShortBio { get; set; }


        public virtual ICollection<Book> Books { get; set; }

        private Author()
        {
            
        }
        internal Author(
            Guid id,
            string name,
            DateTime birthDate,
            string shortBio)
            :base(id)
        {
           SetName(name);
            BirthDate = birthDate;
            ShortBio = shortBio;

              
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: AuthorConsts.MaxNameLength);
        }

        internal Author ChangeName(string name)
        {
            SetName(name);
            return this;
        }
    }
}
