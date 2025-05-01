using Application.Modules.Comments;

namespace Infrastructure.Services;

public interface ICommentsHubService
{
    Task SendCommentCreated(CommentDto comment);

    Task SendCommentUpdated(CommentDto comment);

    Task SendCommentDeleted(Guid commentId);
}
