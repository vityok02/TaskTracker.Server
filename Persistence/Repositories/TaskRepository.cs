using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Domain.Models;
using Persistence.Abstractions;
using Persistence.Repositories.Base;
using Z.Dapper.Plus;

namespace Persistence.Repositories;

public class TaskRepository
    : BaseRepository<TaskEntity, Guid>, ITaskRepository
{
    public TaskRepository(ISqlConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<bool> ExistsByNameForProjectAsync(string name, Guid projectId)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            SELECT COUNT(1) 
            FROM [Task] t
            WHERE t.Name = @Name AND t.ProjectId = @ProjectId";

        return await connection
            .ExecuteScalarAsync<bool>(
                query,
                new
                {
                    Name = name,
                    ProjectId = projectId
                });
    }

    public async Task<IEnumerable<TaskModel>> GetAllExtendedAsync(
        Guid ProjectId,
        string? searchTerm)
    {
        using var connection = ConnectionFactory.Create();

        var query = $@"{GetSelectQuery("p.Id = @ProjectId AND t.Name LIKE @SearchTerm")}
            ORDER BY t.SortOrder";

        return await connection
            .QueryAsync<TaskModel>(
                query,
                new
                {
                    ProjectId,
                    SearchTerm = $"%{searchTerm}%"
                });
    }

    public async Task<IEnumerable<TaskEntity>> GetAllByStateId(Guid stateId)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            SELECT *
            FROM [Task]
            WHERE StateId = @StateId
            ORDER BY SortOrder";

        return await connection
            .QueryAsync<TaskEntity>(
                query,
                new { StateId = stateId });
    }

    public async Task<TaskModel?> GetExtendedByIdAsync(Guid id)
    {
        using var connection = ConnectionFactory.Create();

        var query = GetSelectQuery("t.Id = @Id");

        return await connection
            .QueryFirstOrDefaultAsync<TaskModel>(
                query,
                new { Id = id });
    }

    public async Task<int> GetLastOrderAsync(Guid stateId)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            SELECT MAX(t.SortOrder) 
            FROM [Task] t
            WHERE t.StateId = @StateId";

        return await connection
            .ExecuteScalarAsync<int>(
                query,
                new { StateId = stateId });
    }

    public async Task UpdateRangeAsync(IEnumerable<TaskEntity> tasks)
    {
        var connection = ConnectionFactory.Create();

        await connection.BulkUpdateAsync(tasks);
    }

    private static string GetSelectQuery(string whereCondition) => $@"
            SELECT t.Id, t.Name, t.Description, t.StateId, t.ProjectId,
                t.CreatedAt, t.CreatedBy, t.UpdatedAt, t.UpdatedBy, t.SortOrder, t.StartDate, t.EndDate,
                p.Name AS ProjectName,
                s.Name AS StateName,
                uc.Username AS CreatedByName,
                uu.Username AS UpdatedByName
            FROM [Task] t
            JOIN [Project] p ON p.Id = t.ProjectId
            JOIN [State] s ON s.Id = t.StateId
            JOIN [User] uc ON uc.Id = t.CreatedBy
            LEFT JOIN [User] uu ON uu.Id = t.UpdatedBy
            WHERE {whereCondition}";
}
