using Application.Abstract.Messaging;

namespace Application.Modules.Comments.GetCommentById;

public sealed record GetCommentByIdQuery(Guid CommentId)
    : IQuery<CommentDto>;
