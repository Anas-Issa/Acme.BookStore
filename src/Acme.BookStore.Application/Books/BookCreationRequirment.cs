using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BookStore.Books
{
    public class BookCreationRequirment :IAuthorizationRequirement
    {

    }
}
