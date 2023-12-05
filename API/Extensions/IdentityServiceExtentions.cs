using System.Text;
using Core.Entities.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    /// <summary>
    /// This class is a FRAMEWORK to setup the Identity
    /// </summary>
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            var builder = services.AddIdentityCore<AppUser>();

            builder = new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddEntityFrameworkStores<AppIdentityDbContext>();
            builder.AddSignInManager<SignInManager<AppUser>>();

            // Since the SignInManager RELIES on AUTHENTICATION, 
            // we needed to add the LINE below
            // TO DO: COMEBACK to configure the AUTHENTICAION LATER
            // services.AddAuthentication();
            
            //configure our Authentication services 
            //& tell Authentication services what type of Authentication we're using 
            //& how Authentication services needs to validate the token
            //& pass the token to the client
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(optios => {
                optios.TokenValidationParameters=new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                    ValidIssuer = config["Token:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false
                };
            });

            return services;
        }
    }
}
