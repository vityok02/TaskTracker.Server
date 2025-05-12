using Application.Abstract.Interfaces.Repositories;
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
            .AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IProjectRepository, ProjectRepository>()
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<ITaskRepository, TaskRepository>()
            .AddScoped<IStateRepository, StateRepository>()
            .AddScoped<ICommentRepository, CommentRepository>()
            .AddSingleton<IConnectionStringProvider, ConnectionStringProvider>()
            .AddScoped<ProjectMemberRepository>()
            .AddScoped<IProjectMemberRepository, CachedProjectMemberRepository>()
            .AddScoped<ITagRepository, TagRepository>()

            ;

        return services;
    }
}
