using Api.Controllers.Comment.Responses;

namespace Api.Hubs;

public interface ICommentsHub
{
    Task ReceiveCommentCreated(CommentResponse comment);

    Task ReceiveCommentUpdated(CommentResponse comment);

    Task ReceiveCommentDeleted(Guid commentId);
}