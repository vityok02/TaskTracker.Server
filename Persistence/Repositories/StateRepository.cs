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

    public async Task<(StateEntity state1, StateEntity state2)> GetBothByIdsAsync(
        Guid stateId1, Guid stateId2)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            SELECT *
            FROM [State]
            WHERE Id = @StateId1 OR Id = @StateId2";

        var states = (await connection
            .QueryAsync<StateEntity>(
                query,
                new 
                {
                    StateId1 = stateId1,
                    StateId2 = stateId2 
                }))
            .ToList();

        return (states[0], states[1]);
    }

    public async Task UpdateBothAsync(StateEntity state1, StateEntity state2)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            UPDATE [State]
            SET Number = @Number1, UpdatedAt = @UpdatedAt1, UpdatedBy = @UpdatedBy1
            WHERE Id = @Id1;
            UPDATE [State]
            SET Number = @Number2, UpdatedAt = @UpdatedAt2, UpdatedBy = @UpdatedBy2
            WHERE Id = @Id2;";

        await connection.ExecuteAsync(
            query,
            new
            {
                Id1 = state1.Id,
                Number1 = state1.Number,
                UpdatedAt1 = state1.UpdatedAt,
                UpdatedBy1 = state1.UpdatedBy,

                Id2 = state2.Id,
                Number2 = state2.Number,
                UpdatedBy2 = state2.UpdatedBy,
                UpdatedAt2 = state2.UpdatedAt
            });
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