using Acme.BookStore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Acme.BookStore.Members
{
    public class EfCoreMemberRepository : EfCoreRepository<BookStoreDbContext, Member, Guid>, IMemberRepository
    {
        public EfCoreMemberRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public override async Task<IQueryable<Member>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }

    }
    public static class MemberQueryableExtension
    {
        public static IQueryable<Member> IncludeDetails(
            this IQueryable<Member> queryable,
            bool include = true)
        {
            if (!include)
            {
                return queryable;
            }
            return queryable
                .Include(m => m.MemberBooks)
                .ThenInclude(m=>m.Book);
        }
    }
}
