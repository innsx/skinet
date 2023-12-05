using System.Threading.Tasks;
using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                // creating a loggerFactory object to handle exceptions
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var context = services.GetRequiredService<StoreContext>();

                    // Recreating Skinet Database if one DOES NOT ALREADY EXISTED
                    await context.Database.MigrateAsync();

                    // Seeding the Products, Brands, ProductTypes Tables
                    await StoreContextSeed.SeedAsync(context, loggerFactory);

                    // from this program class, we pass in the UserManager into 
                    // AppIdentityDbContextSeed class
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var identityContext = services.GetRequiredService<AppIdentityDbContext>();

                    // creating the database
                    await identityContext.Database.MigrateAsync();

                    // seeding the AppUser database
                    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
                }
                catch (System.Exception ex)
                {
                    // handling exceptions
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An Error occured during migrations");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
