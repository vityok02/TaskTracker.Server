using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        return services;
    }
}
