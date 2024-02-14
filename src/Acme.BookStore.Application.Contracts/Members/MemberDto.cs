using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Members
{
    public  class MemberDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }

        public string Notes { get; set; }

        public List<MemberBooksDto> MemberBooks { get; set; }
    }
}
