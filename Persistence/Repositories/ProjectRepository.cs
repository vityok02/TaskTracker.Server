using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain;
using Microsoft.Data.SqlClient;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class ProjectRepository<TId> : GenericRepository<Project, TId>, IProjectRepository<TId>
{
    public ProjectRepository(ISqlConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task<TId> CreateAsync(Guid userId, Project project)
    {
        using var connection = ConnectionFactory.Create();

        var projectId = await connection.InsertAsync<TId, Project>(project)
            ?? throw new InvalidOperationException("Project not created");

        var role = await connection.QueryFirstOrDefaultAsync<Role>("select * from Role")
            ?? throw new InvalidOperationException("Role not found");

        await connection.ExecuteAsync(
            "INSERT INTO ProjectMember(UserId, ProjectId, RoleId) VALUES (@UserId, @ProjectId, @RoleId)",
            new { UserId = userId, ProjectId = projectId, RoleId = role.Id });

        return projectId;
    }
}
