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

        var taskDictionary = new Dictionary<Guid, TaskModel>();

        var result = await connection
            .QueryAsync<TaskModel, TagEntity, TaskModel>(
                query,
                (task, tag) =>
                {
                    if (!taskDictionary.TryGetValue(task.Id, out var taskEntry))
                    {
                        taskEntry = task;
                        taskEntry.Tags = [];
                        taskDictionary.Add(taskEntry.Id, taskEntry);
                    }

                    if (tag != null)
                    {
                        taskEntry.Tags.Add(tag);
                    }

                    return taskEntry;
                },
                new
                {
                    ProjectId,
                    SearchTerm = $"%{searchTerm}%"
                },
                splitOn: "Id");

        return taskDictionary.Values;
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

    public async Task AddTagAsync(Guid taskId, Guid tagId)
    {
        var connection = ConnectionFactory.Create();

        var query = @"INSERT INTO [TaskTag](TaskId, TagId)
            VALUES(@TaskId, @TagId)";

        await connection.ExecuteAsync(
            query,
            new
            {
                TaskId = taskId,
                TagId = tagId
            });
    }

    public async Task RemoveTagAsync(Guid taskId, Guid tagId)
    {
        var connection = ConnectionFactory.Create();

        var query = @"DELETE FROM [TaskTag]
            WHERE TaskId = @TaskId AND TagId = @TagId";

        await connection.ExecuteAsync(
            query,
            new
            {
                TaskId = taskId,
                TagId = tagId
            });
    }

    public async Task GetTagsAsync(Guid taskId)
    {
        var connection = ConnectionFactory.Create();

        var query = @"SELECT * FROM [Tag]
            LEFT JOIN [TaskTag] tt ON tt.TagId = Id
            WHERE tt.TaskId = @TaskId";

        await connection.QueryAsync(
            query,
            new { TaskId = taskId });
    }

    private static string GetSelectQuery(string whereCondition) => $@"
            SELECT t.Id, t.Name, t.Description, t.StateId, t.ProjectId,
                t.CreatedAt, t.CreatedBy, t.UpdatedAt, t.UpdatedBy, t.SortOrder, t.StartDate, t.EndDate,
                p.Name AS ProjectName,
                s.Name AS StateName,
                s.Color AS StateColor,
                tg.Id,
                tg.Name AS TagName,
                tg.Color,
                tg.SortOrder AS TagSortOrder,
                uc.Username AS CreatedByName,
                uu.Username AS UpdatedByName
            FROM [Task] t
            JOIN [Project] p ON p.Id = t.ProjectId
            JOIN [State] s ON s.Id = t.StateId
            JOIN [User] uc ON uc.Id = t.CreatedBy
            LEFT JOIN [User] uu ON uu.Id = t.UpdatedBy
            LEFT JOIN [TaskTag] tt ON tt.TaskId = t.Id
            LEFT JOIN [Tag] tg ON tg.Id = tt.TagId
            WHERE {whereCondition}";
}
