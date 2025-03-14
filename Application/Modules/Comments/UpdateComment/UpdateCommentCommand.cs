using Application.Abstract.Messaging;

namespace Application.Modules.Comments.UpdateComment;

public sealed record UpdateCommentCommand(
    Guid CommentId,
    string Comment,
    Guid UserId)
    : ICommand;
