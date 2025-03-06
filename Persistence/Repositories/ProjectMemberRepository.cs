using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
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

    public async Task<ProjectMemberEntity?> GetAsync(
        Guid userId,
        Guid projectId)
    {
        using var connection = _connectionFactory.Create();

        var query = @"
            SELECT * FROM ProjectMember pm 
            WHERE pm.ProjectId = @ProjectId AND pm.UserId = @UserId";

        return await connection
            .QueryFirstOrDefaultAsync<ProjectMemberEntity>(query,
                new { UserId = userId, ProjectId = projectId });
    }

    public async Task<ProjectMemberModel?> GetExtendedAsync(
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

    public async Task UpdateAsync(ProjectMemberEntity projectMember)
    {
        using var connection = _connectionFactory.Create();

        var query = @"UPDATE ProjectMember
            SET RoleId = @RoleId
            WHERE UserId = @UserId AND ProjectId = @ProjectId";

        await connection
            .ExecuteAsync(query, projectMember);
    }

    public async Task DeleteAsync(ProjectMemberEntity projectMember)
    {
        using var connection = _connectionFactory.Create();

        var query = @"DELETE FROM ProjectMember
            WHERE UserId = @UserId AND ProjectId = @ProjectId";

        await connection
            .ExecuteAsync(query, projectMember);
    }
}
