using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Persistence.Abstractions;

namespace Persistence.Repositories;

public class ProjectMemberRepository
    : IProjectMemberRepository
{
    private const string SelectQuery = @"SELECT * FROM [Project] p
            JOIN [ProjectMember] pm ON p.Id = pm.ProjectId
            JOIN [User] u ON u.Id = pm.UserId
            WHERE p.Id = @ProjectId AND pm.UserId = @UserId";

    private readonly ISqlConnectionFactory _connectionFactory;

    public ProjectMemberRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task CreateMember(
        Guid userId,
        Guid projectId,
        Guid roleId)
    {
        using var connection = _connectionFactory.Create();

        var query = @"INSERT INTO ProjectMember(UserId, ProjectId, RoleId)
            VALUES(@UserId, @ProjectId, @RoleId)";

        await connection
            .ExecuteAsync(query, 
                new { UserId = userId, ProjectId = projectId, RoleId = roleId });
    }

    public async Task<IEnumerable<ProjectMember>> GetAllAsync(
        Guid userId,
        Guid projectId)
    {
        using var connection = _connectionFactory.Create();

        var query = SelectQuery;

        return await connection
            .QueryAsync<ProjectMember>(query,
                new { UserId = userId, ProjectId = projectId });
    }

    public async Task<ProjectMember?> GetAsync(
        Guid userId,
        Guid projectId)
    {
        using var connection = _connectionFactory.Create();

        var query = SelectQuery;

        return await connection
            .QueryFirstOrDefaultAsync<ProjectMember>(query,
                new { UserId = userId, ProjectId = projectId });
    }

    public async Task<Role> GetMemberRole(
        Guid userId,
        Guid projectId)
    {
        using var connection = _connectionFactory.Create();

        var query = @"SELECT r.* FROM [Role] r
            JOIN [ProjectMember] pm ON r.Id = pm.RoleId
            WHERE pm.UserId = @UserId AND pm.ProjectId = @ProjectId";

        return await connection
            .QueryFirstAsync<Role>(query, 
                new { UserId = userId, ProjectId = projectId });
    }
}
