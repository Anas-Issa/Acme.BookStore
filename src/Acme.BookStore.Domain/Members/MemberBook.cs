using Acme.BookStore.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Acme.BookStore.Members
{
    public  class MemberBook : Entity<Guid>
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid MemberId { get; set; }

        public Member Member  { get; set; }

        public DateTime BorrowingDate { get; set; }

        public DateTime  ReturnDate { get; set; }

        //public override object?[] GetKeys()
        //{
        //    return new object[] {BookId,MemberId};
        //}

    }
}
