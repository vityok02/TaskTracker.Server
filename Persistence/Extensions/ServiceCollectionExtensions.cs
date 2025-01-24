using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Abstractions;
using Persistence.Repositories;

namespace Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<AppDbContext>(p => 
                new AppDbContext(configuration.GetConnectionString("localdb")!))
            .AddScoped(typeof(IEntityAttributeValuesProvider<>), typeof(EntityAttributeValuesProvider<>))
            .AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

        return services;
    }
}
