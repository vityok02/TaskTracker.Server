using Api.Controllers.Comment.Responses;

namespace Api.Services;

public interface ICommentsHubService
{
    Task SendCommentCreated(CommentResponse comment);

    Task SendCommentUpdated(CommentResponse comment);

    Task SendCommentDeleted(Guid commentId);
}
