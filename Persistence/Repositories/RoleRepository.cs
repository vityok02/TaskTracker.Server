using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Persistence.Abstractions;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class RoleRepository
    : BaseRepository<RoleEntity, Guid>, IRoleRepository
{
    public RoleRepository(ISqlConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<RoleEntity?> GetByNameAsync(string name)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"SELECT * FROM [Role]
            WHERE [Name] = @Name";

        return await connection
            .QueryFirstAsync<RoleEntity>(query, new { Name = name });
    }

    public async Task<RoleEntity> GetMemberRole(
    Guid userId,
    Guid projectId)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"SELECT r.* FROM [Role] r
            JOIN [ProjectMember] pm ON r.Id = pm.RoleId
            WHERE pm.UserId = @UserId AND pm.ProjectId = @ProjectId";

        return await connection
            .QueryFirstAsync<RoleEntity>(query,
                new { UserId = userId, ProjectId = projectId });
    }
}
