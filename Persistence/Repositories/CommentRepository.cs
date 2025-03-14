using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Domain.Models;
using Persistence.Abstractions;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class CommentRepository
    : BaseRepository<CommentEntity, Guid>, ICommentRepository
{
    public CommentRepository(ISqlConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<CommentModel?> GetExtendedByIdAsync(Guid id)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            SELECT
                c.Id,
                c.Comment,
                c.TaskId,
                c.CreatedBy,
                c.CreatedAt,
                c.UpdatedBy,
                c.UpdatedAt,
                t.Name AS TaskName,
                uc.Username AS CreatedByName,
                uu.Username AS UpdatedByName
            FROM [Comment] c
            JOIN [Task] t ON c.TaskId = t.Id
            JOIN [User] uc ON c.CreatedBy = uc.Id
            LEFT JOIN [User] uu ON c.UpdatedBy = uu.Id
            WHERE c.Id = @Id";

        return await connection
            .QuerySingleOrDefaultAsync<CommentModel>(
                query,
                new { Id = id });
    }

    public async Task<IEnumerable<CommentModel>> GetAllExtendedByTaskIdAsync(Guid taskId)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            SELECT
                c.Id,
                c.Comment,
                c.TaskId,
                c.CreatedBy,
                c.CreatedAt,
                c.UpdatedBy,
                c.UpdatedAt,
                t.Name AS TaskName,
                uc.Username AS CreatedByName,
                uu.Username AS UpdatedByName
            FROM [Comment] c
            JOIN [Task] t ON c.TaskId = t.Id
            JOIN [User] uc ON c.CreatedBy = uc.Id
            LEFT JOIN [User] uu ON c.UpdatedBy = uu.Id
            WHERE c.TaskId = @TaskId
            ORDER BY c.CreatedAt";

        return await connection
            .QueryAsync<CommentModel>(
                query,
                new { TaskId = taskId });
    }
}
