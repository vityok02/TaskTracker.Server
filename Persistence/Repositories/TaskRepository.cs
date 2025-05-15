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

    public async Task<IEnumerable<TaskTagEntity>> GetTagsAsync(Guid taskId)
    {
        var connection = ConnectionFactory.Create();

        var query = @"SELECT * FROM [TaskTag]
            WHERE TaskId = @TaskId
            AND TaskId = @TaskId";

        return await connection
            .QueryAsync<TaskTagEntity>(
                query,
                new
                {
                    TaskId = taskId,
                });
    }

    public async Task<IEnumerable<TaskModel>> GetAllExtendedAsync(
        Guid projectId,
        string? searchTerm,
        IEnumerable<Guid>? tagIds)
    {
        // TODO: do via query but not linq.
        using var connection = ConnectionFactory.Create();

        var filters = new List<string> { "p.Id = @ProjectId" };

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            filters.Add("t.Name LIKE @SearchTerm OR tg.Name LIKE @SearchTerm");
        }

        if (tagIds != null && tagIds.Any())
        {
            filters.Add(@"
                EXISTS (
                    SELECT 1 FROM [TaskTag] tt
                    WHERE tt.TaskId = t.Id AND tt.TagId IN @TagIds
                )");
        }

        var query = $@"{GetSelectQuery(string.Join(" AND ", filters))}
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
                    projectId,
                    SearchTerm = $"%{searchTerm}%",
                    TagIds = tagIds
                },
                splitOn: "Id");

        var tasks = taskDictionary.Values;

        // Якщо передані теги — відфільтруй по них
        if (tagIds != null && tagIds.Any())
        {
            var tagIdSet = tagIds.ToHashSet();
            return tasks
                .Where(task => task.Tags.Select(t => t.Id).ToHashSet().IsSupersetOf(tagIdSet));
        }

        return tasks;
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
                    Id = id
                },
                splitOn: "Id");

        return taskDictionary.Values
            .SingleOrDefault();
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

    public async Task AddTagAsync(TaskTagEntity taskTag)
    {
        var connection = ConnectionFactory.Create();

        var query = @"INSERT INTO [TaskTag](TaskId, TagId)
            VALUES(@TaskId, @TagId)";

        await connection.ExecuteAsync(
            query,
            new
            {
                taskTag.TaskId,
                taskTag.TagId
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

    public async Task<TaskTagEntity?> GetTaskTagAsync(Guid taskId, Guid tagId)
    {
        var connection = ConnectionFactory.Create();

        var query = @"SELECT * FROM [TaskTag]
            WHERE TaskId = @TaskId
            AND TagId = @TagId";

        return await connection
            .QuerySingleOrDefaultAsync<TaskTagEntity?>(
                query,
                new
                {
                    TaskId = taskId,
                    TagId = tagId,
                });
    }

    private static string GetSelectQuery(string whereCondition) => $@"
            SELECT t.Id, t.Name, t.Description, t.StateId, t.ProjectId,
                t.CreatedAt, t.CreatedBy, t.UpdatedAt, t.UpdatedBy, t.SortOrder, t.StartDate, t.EndDate,
                uc.Username AS CreatedByName,
                uu.Username AS UpdatedByName,
                p.Name AS ProjectName,
                s.Name AS StateName,
                s.Color AS StateColor,
                tg.Id,
                tg.Name,
                tg.Color,
                tt.SortOrder
            FROM [Task] t
            JOIN [Project] p ON p.Id = t.ProjectId
            JOIN [State] s ON s.Id = t.StateId
            JOIN [User] uc ON uc.Id = t.CreatedBy
            LEFT JOIN [User] uu ON uu.Id = t.UpdatedBy
            LEFT JOIN [TaskTag] tt ON tt.TaskId = t.Id
            LEFT JOIN [Tag] tg ON tg.Id = tt.TagId
            WHERE {whereCondition}";
}
