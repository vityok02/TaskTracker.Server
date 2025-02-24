using Application.Abstract.Interfaces;
using Infrastructure.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services
            .AddScoped<IJwtProvider, JwtProvider>()
            .AddScoped<IDateTimeProvider, DateTimeService>()
            .AddScoped<IPasswordHasher, PasswordHasher>()
            .AddScoped<IResetTokenService, ResetTokenProvider>()
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<IUserManager, UserManager>()
            ;
    }
}
