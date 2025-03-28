using Api.OptionsSetup;
using Infrastructure;
using Infrastructure.Authentication;
using Infrastructure.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography.Xml;
using System.Text;

namespace Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services
            .AddAutoMapper(AssemblyReference.Assembly)
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.SchemaFilter<ProblemDetailsSchemaFilter>();

                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer"
                    });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            })
            .AddControllers()
            ;

        return services;
    }

    public static IServiceCollection AddAuthConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<ClientOptions>(configuration.GetSection("ClientSettings"));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(x =>
             {
                 var config = configuration.GetSection("Jwt").Get<JwtOptions>() ??
                    throw new InvalidOperationException("JWT configuration is missing");

                 x.TokenValidationParameters = new()
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = config.Issuer,
                     ValidAudience = config.Audience,
                     IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config.SecretKey)),
                 };
             });

        return services;
    }
}
