using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BookStore.Books
{
    public class BookCreateionRequiremantHandler : AuthorizationHandler<BookCreationRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BookCreationRequirment requirement)
        {
            if (context.User.HasClaim(c => c.Type == "Age"))
                {
                context.Succeed(requirement);
                  }
            return Task.CompletedTask;
        }
    }
}
