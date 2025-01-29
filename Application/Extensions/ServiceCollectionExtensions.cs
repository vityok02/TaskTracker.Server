using Application.Abstract.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly))
            .AddScoped<IDateTimeService, DateTimeService>()
            .AddScoped<IPasswordHasher, PasswordHasher>()
            ;

        return services;
    }
}
