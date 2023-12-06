using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SkiNet API", Version = "v1" });
    
                // adding an Authenticate Icon/logo inside Swagger 
                // next we so a POST request to create a Account/Login to generate a TOKEN
                // we'll copy the generated TOKEN by opening the Authenticate Icon/logo
                // paste the COPIED TOKEN inside the popup windows and close this popup windows
                // NOW, the TOKEN is SAVED & STORED inside SWAGGER
                // launch ANY request to TEST any ENDPOINTS using SWAGGER instead of any others testing software/tools
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Auth Bearer Scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement
                    {
                        {
                            securitySchema,
                            new[] {"Bearer"}
                        }
                    };

                c.AddSecurityRequirement(securityRequirement);
            });

            return services;
        }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkiNet API v1");
        });

        return app;
    }
}
}



