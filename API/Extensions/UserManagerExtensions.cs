using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindUserByClaimsPrincipalWithAddressAsync(this UserManager<AppUser> input, ClaimsPrincipal user)   
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;


            var appUserByClaimsPrincipalWithAddress =  await input.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);

            return appUserByClaimsPrincipalWithAddress;
        }

        public static async Task<AppUser> FindByEmailFromClaimsPrincipal(this UserManager<AppUser> input, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            var appUserEmailFromClaims =  await input.Users.SingleOrDefaultAsync(x => x.Email == email);

            return appUserEmailFromClaims;
        }
    }
}
