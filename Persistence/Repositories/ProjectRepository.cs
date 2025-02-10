using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Persistence.Abstractions;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class ProjectRepository
    : BaseRepository<Project, Guid>, IProjectRepository
{
    public ProjectRepository(ISqlConnectionFactory connectionFactory)
        : base(connectionFactory)
    { }

    public async Task<bool> ExistsByNameAsync(Guid userId, string projectName)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"SELECT COUNT(1) FROM [Project] p
            JOIN [ProjectMember] pm ON p.Id = pm.ProjectId
            JOIN [User] u ON u.Id = pm.UserId
            WHERE p.Name = @Name AND pm.UserId = @UserId";

        var result = await connection
            .ExecuteScalarAsync<bool>(query, new { Name = projectName, UserId = userId });

        return result;
    }

    public async Task<Guid> CreateAsync(Project project, Guid roleId)
    {
        using var connection = ConnectionFactory
            .Create();

        var projectId = await connection
            .InsertAsync<Guid, Project>(project);

        await connection.ExecuteAsync(
            @"INSERT INTO ProjectMember(UserId, ProjectId, RoleId) 
                VALUES (@UserId, @ProjectId, @RoleId)",
            new { UserId = project.CreatedBy, ProjectId = projectId, RoleId = roleId });

        return projectId;
    }

    public async Task<IEnumerable<Project>> GetAllAsync(Guid userId)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"SELECT p.* FROM [Project] p
            JOIN [ProjectMember] pm ON p.Id = pm.ProjectId
            WHERE pm.UserId = @UserId";

        return await connection
            .QueryAsync<Project>(query, new { UserId = userId });
    }
}
