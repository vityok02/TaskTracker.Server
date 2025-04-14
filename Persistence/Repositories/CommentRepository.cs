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
                t.Name AS TaskName,
                c.CreatedAt,
                c.UpdatedAt,

                uc.Id,
                uc.Username AS Name,
                uc.AvatarUrl,

                uu.Id,
                uu.Username AS Name,
                uu.AvatarUrl
            FROM [Comment] c
            JOIN [Task] t ON c.TaskId = t.Id
            JOIN [User] uc ON c.CreatedBy = uc.Id
            LEFT JOIN [User] uu ON c.UpdatedBy = uu.Id
            WHERE c.Id = @Id";

        var comments = await connection
            .QueryAsync<CommentModel, UserInfoModel, UserInfoModel, CommentModel>(
                sql: query,
                map: (comment, createdBy, updatedBy) =>
                {
                    comment.CreatedBy = createdBy;
                    comment.UpdatedBy = updatedBy;
                    return comment;
                },
                param: new { Id = id },
                splitOn: "Id, Id");

        return comments.SingleOrDefault();
    }

    public async Task<IEnumerable<CommentModel>> GetAllExtendedByTaskIdAsync(Guid taskId)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
        SELECT
            c.Id,
            c.Comment,
            c.TaskId,
            t.Name AS TaskName,
            c.CreatedAt,
            c.UpdatedAt,

            uc.Id,
            uc.Username AS Name,
            uc.AvatarUrl,

            uu.Id,
            uu.Username AS Name,
            uu.AvatarUrl
        FROM [Comment] c
        JOIN [Task] t ON c.TaskId = t.Id
        JOIN [User] uc ON c.CreatedBy = uc.Id
        LEFT JOIN [User] uu ON c.UpdatedBy = uu.Id
        WHERE c.TaskId = @TaskId
        ORDER BY c.CreatedAt";

        var comments = await connection
            .QueryAsync<CommentModel, UserInfoModel, UserInfoModel, CommentModel>(
                sql: query,
                map: (comment, createdBy, updatedBy) =>
                {
                    comment.CreatedBy = createdBy;
                    comment.UpdatedBy = updatedBy;
                    return comment;
                },
                param: new { TaskId = taskId },
                splitOn: "Id, Id");

        return comments;
    }
}
