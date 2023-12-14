using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string RetrieveEmailFromClaimPrincipal(this ClaimsPrincipal claimsPrincipal)
        {
            var userEmail = claimsPrincipal?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            return userEmail;
        }
    }
}
