using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Acme.BookStore.Members
{
    public  class Member : AuditedEntity<Guid>,IMultiTenant
    {
        public Guid? TenantId { get; set; } 
        public string Name { get; set; }
        public string Notes { get; set; }

        public virtual  ICollection<MemberBook> MemberBooks { get; set; }




    }
}
