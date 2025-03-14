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

    public async Task<IEnumerable<StateModel>> GetAllByProjectIdAsync(Guid projectId)
    {
        using var connection = ConnectionFactory.Create();

        var query = GetSelectQuery("WHERE s.ProjectId = @ProjectId");

        return await connection
            .QueryAsync<StateModel>(
                query,
                new { ProjectId = projectId });
    }

    public async Task<StateModel?> GetExtendedByIdAsync(Guid id)
    {
        using var connection = ConnectionFactory.Create();

        return await connection
            .QueryFirstOrDefaultAsync<StateModel>(
                GetSelectQuery("WHERE s.Id = @Id"),
                new { Id = id });
    }

    public async Task<int> GetLastStateNumberAsync(Guid ProjectId)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            SELECT MAX(Number) 
            FROM [State] 
            WHERE ProjectId = @ProjectId";

        return await connection
            .QueryFirstOrDefaultAsync<int>(
                query,
                new { ProjectId });
    }

    private static string GetSelectQuery(string whereCondition) => @$"
        SELECT
            s.Id,
            s.Number,
            s.Name,
            s.Description,
            s.CreatedBy,
            s.CreatedAt,
            s.UpdatedBy,
            s.UpdatedAt,
            uc.Username AS CreatedByName,
            uu.Username AS UpdatedByName
        FROM [State] s
        LEFT JOIN [User] uc ON uc.Id = s.CreatedBy
        LEFT JOIN [User] uu ON uu.Id = s.UpdatedBy
        {whereCondition}";
}