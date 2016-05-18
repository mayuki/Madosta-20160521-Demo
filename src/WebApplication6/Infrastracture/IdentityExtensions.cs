using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication6.Infrastracture
{
    public static class IdentityExtensions
    {
        public static string GetUserId(this ClaimsIdentity identity)
        {
            return identity.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
