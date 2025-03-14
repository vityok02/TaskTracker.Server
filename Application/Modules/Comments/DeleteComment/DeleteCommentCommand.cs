using Application.Abstract.Messaging;

namespace Application.Modules.Comments.DeleteComment;

public sealed record DeleteCommentCommand(
    Guid CommentId,
    Guid UserId)
    : ICommand;
