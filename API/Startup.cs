using API.Extensions;
using API.Helpers;
using API.Middleware;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<AppIdentityDbContext>(x => 
            {
                x.UseSqlite(_config.GetConnectionString("IdentityConnection"));
            });

            // Adding AutoMapper as a service
            services.AddAutoMapper(typeof(MappingProfiles));

            // Adding a customized API Validation Error Response as a Service            
            services.AddApplicationServices();

            // adding Swagger as a service
            services.AddSwaggerDocumentation();

            // IdentityServiceExtensions.cs class
            services.AddIdentityServices(_config); 

            // Adding CORS: CROSS ORIGIN RESOURCES SHARING as a SERVICE
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });

            services.AddSingleton<IConnectionMultiplexer>(c => {
                var configuration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);

                return ConnectionMultiplexer.Connect(configuration);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //*****************************************
            // comments this if statements & use our customized
            // exceptionMiddleware statement below
            //*****************************************
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            app.UseMiddleware<ExceptionMiddleware>();

            // Global-Error-Handler for ANY UNMATCHED StatusCode
            // or DO NOT EXISTED ENDPOINT in ErrorController class
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            // Implements CORS Service's policy
            app.UseCors("CorsPolicy"); 

            // add Authentication in the Middleware pipeline
            app.UseAuthentication();

            app.UseAuthorization();

            // added swagger as a part of the middleware pipeline            
            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
