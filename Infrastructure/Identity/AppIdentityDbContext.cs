using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        // specified with a type of <AppIdentityDbContext>
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // needed to add this line for Identity and the PRIMARY Key that
            // Identity is using for the ID that is is using for AppUser field
            base.OnModelCreating(builder); 
        }
    }
}