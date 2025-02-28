using Application.Abstract.Interfaces.Repositories;
using Azure.Core;
using Dapper;
using Domain.Entities;
using Domain.Models;
using Persistence.Abstractions;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class TaskRepository : BaseRepository<TaskEntity, Guid>, ITaskRepository
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

    public async Task<IEnumerable<TaskModel>> GetAllByProjectIdAsync(Guid ProjectId)
    {
        using var connection = ConnectionFactory.Create();

        var query = GetSelectQuery("p.Id = @ProjectId");

        return await connection
            .QueryAsync<TaskModel>(
                query,
                new { ProjectId });
    }

    async Task<TaskModel?> ITaskRepository.GetExtendedByIdAsync(Guid id)
    {
        using var connection = ConnectionFactory.Create();

        var query = GetSelectQuery("t.Id = @Id");

        return await connection
            .QueryFirstOrDefaultAsync<TaskModel>(
                query,
                new { Id = id });
    }

    private static string GetSelectQuery(string whereClause) => $@"
            SELECT t.Id, t.Name, t.Description, t.StateId, t.ProjectId,
                t.CreatedAt, t.CreatedBy, t.UpdatedAt, t.UpdatedBy,
                p.Name AS ProjectName,
                s.Name AS State,
                uc.Username AS CreatedByName,
                uu.Username AS UpdatedByName
            FROM [Task] t
            JOIN [Project] p ON p.Id = t.ProjectId
            JOIN [State] s ON s.Id = t.StateId
            JOIN [User] uc ON uc.Id = t.CreatedBy
            LEFT JOIN [User] uu ON uu.Id = t.UpdatedBy
            WHERE {whereClause}";
}
