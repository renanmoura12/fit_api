using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace api_fit.Services
{
    public static class SwaggerService
    {
        public static IServiceCollection SwaggerInfra(this IServiceCollection services, IConfiguration config)
        {
            services.AddSwaggerGen(a =>
                {
                    a.SwaggerDoc("v1", new OpenApiInfo { Title = "FIT API", Version = "v1" });
                    a.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Insira o Token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer"
                    });
                    a.AddSecurityRequirement(new OpenApiSecurityRequirement{
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference =  new OpenApiReference
                                {
                                        Type=ReferenceType.SecurityScheme,
                                        Id="Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });

                });

            services.AddAuthentication(
                x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            )
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["jwt:key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }
    }
}
