using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Persistence.Repositories.Base;
using System.Data;
using System.Transactions;

namespace Persistence.Repositories;

public class ProjectRepository
    : BaseRepository<Project, Guid>, IProjectRepository
{
    public ProjectRepository(ISqlConnectionFactory connectionFactory) : base(connectionFactory)
    { }

    public async Task<bool> ExistsAsync(Guid userId, string projectName)
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

    public async Task<Guid> CreateAsync(Guid userId, Project project)
    {
        using var connection = ConnectionFactory
            .Create();
       
        var projectId = await connection
            .InsertAsync<Guid, Project>(project);

        var roleId = await connection
            .QueryFirstOrDefaultAsync<Guid>("select Id from Role");

        await connection.ExecuteAsync(
            @"INSERT INTO ProjectMember(UserId, ProjectId, RoleId) 
                VALUES (@UserId, @ProjectId, @RoleId)",
            new { UserId = userId, ProjectId = projectId, RoleId = roleId });

        return projectId;
    }
}
