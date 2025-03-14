using Application.Abstract.Messaging;

namespace Application.Modules.Comments.CreateComment;

public sealed record CreateCommentCommand(
    string Comment,
    Guid TaskId,
    Guid UserId)
    : ICommand<CommentDto>;
