using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Domain.Models;
using Persistence.Abstractions;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class StateRepository
    : BaseRepository<StateEntity, Guid>, IStateRepository
{
    private const string SelectQuery = @"
            SELECT s.Id, s.Number, s.Name, s.Description, s.CreatedBy, s.CreatedAt, s.UpdatedBy, s.UpdatedAt,
            uc.Username AS CreatedByName,
            uu.Username AS UpdatedByName
            FROM [State] s
            JOIN [Project] p ON p.Id = s.ProjectId
            JOIN [ProjectMember] pm ON pm.ProjectId = p.Id
            JOIN [User] uc ON uc.Id = s.CreatedBy
            LEFT JOIN [User] uu ON uu.Id = s.UpdatedBy
            WHERE s.ProjectId = @ProjectId";

    public StateRepository(ISqlConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<bool> ExistsForProject(Guid stateId, Guid projectId)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            SELECT COUNT(1)
            FROM [State] s
            WHERE s.ProjectId = @ProjectId AND s.Id = @StateId";

        return await connection
            .ExecuteScalarAsync<bool>(
                query,
                new
                {
                    StateId = stateId,
                    ProjectId = projectId
                });
    }

    public async Task<bool> ExistsByProjectIdAndNameAsync(Guid projectId, string name)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            SELECT COUNT(1)
            FROM [State] s
            WHERE s.ProjectId = @ProjectId AND s.Name = @Name";

        return await connection
            .ExecuteScalarAsync<bool>(
                query,
                new
                {
                    ProjectId = projectId,
                    Name = name
                });
    }

    public async Task<IEnumerable<StateModel>> GetAllByProjectIdAsync(Guid projectId)
    {
        using var connection = ConnectionFactory.Create();

        return await connection
            .QueryAsync<StateModel>(
            SelectQuery,
            new { ProjectId = projectId });
    }
}
