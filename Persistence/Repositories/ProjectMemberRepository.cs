using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain.Models;
using Persistence.Abstractions;

namespace Persistence.Repositories;

public class ProjectMemberRepository
    : IProjectMemberRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public ProjectMemberRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<ProjectMemberModel> CreateAsync(
        Guid userId,
        Guid projectId,
        Guid roleId)
    {
        using var connection = _connectionFactory.Create();

        var insertQuery = @"INSERT INTO ProjectMember(UserId, ProjectId, RoleId)
            VALUES(@UserId, @ProjectId, @RoleId)";

        var selectQuery = GetSelectQuery("p.Id = @ProjectId AND u.Id = @UserId");

        var query = insertQuery + selectQuery;

        return await connection
            .QueryFirstAsync<ProjectMemberModel>(query,
                new
                {
                    UserId = userId,
                    ProjectId = projectId,
                    RoleId = roleId
                });
    }

    public async Task<IEnumerable<ProjectMemberModel>> GetAllAsync(Guid projectId)
    {
        using var connection = _connectionFactory.Create();

        var query = GetSelectQuery("p.Id = @ProjectId");

        return await connection
            .QueryAsync<ProjectMemberModel>(query,
                new { ProjectId = projectId });
    }

    public async Task<ProjectMemberModel?> GetAsync(
        Guid userId,
        Guid projectId)
    {
        using var connection = _connectionFactory.Create();

        var query = GetSelectQuery("p.Id = @ProjectId AND u.Id = @UserId");

        return await connection
            .QueryFirstOrDefaultAsync<ProjectMemberModel>(query,
                new { UserId = userId, ProjectId = projectId });
    }

    private static string GetSelectQuery(string whereClause) => $@"
            SELECT pm.UserId, pm.ProjectId, pm.RoleId,
            p.Name AS ProjectName,
            u.Username AS UserName,
            r.Name AS RoleName
            FROM [ProjectMember] pm
            JOIN [Project] p ON p.Id = pm.ProjectId
            JOIN [User] u ON u.Id = pm.UserId
            JOIN [Role] r ON r.Id = pm.RoleId
            WHERE {whereClause}";
}
