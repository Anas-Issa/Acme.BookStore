using Acme.BookStore.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Acme.BookStore.Members
{
    public  class MemberBook : Entity<Guid>,IMultiTenant
    {
        public Guid BookId { get; set; }
       
        public Guid MemberId { get; set; }

       

        public DateTime BorrowingDate { get; set; }

        public DateTime  ReturnDate { get; set; }

        public Guid? TenantId { get; set; }

        public virtual Book Book { get; set; }
        public virtual Member Member{ get; set; }

        //public override object?[] GetKeys()
        //{
        //    return new object[] {BookId,MemberId};
        //}

    }
}
