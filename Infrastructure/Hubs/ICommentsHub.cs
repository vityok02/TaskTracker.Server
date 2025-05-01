using Application.Modules.Comments;

namespace Infrastructure.Hubs;

public interface ICommentsHub
{
    Task ReceiveCommentCreated(CommentDto comment);

    Task ReceiveCommentUpdated(CommentDto comment);

    Task ReceiveCommentDeleted(Guid commentId);
}