using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
