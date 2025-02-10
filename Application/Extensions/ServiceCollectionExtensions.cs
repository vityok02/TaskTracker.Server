using Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddMediatR(cfg => cfg
                .RegisterServicesFromAssembly(AssemblyReference.Assembly))
            .AddAutoMapper(AssemblyReference.Assembly)
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
            .AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true)
            ;

        return services;
    }
}
