using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Database;

namespace Persistence.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddSingleton(_ => 
        //    new DatabaseInitializer(configuration
        //        .GetConnectionString("localdb")!));

        return services;
    }
}
