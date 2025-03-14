using Application.Abstract.Messaging;

namespace Application.Modules.Comments.GetAllComments;

public sealed record GetAllCommentsQuery(Guid TaskId)
    : IQuery<IEnumerable<CommentDto>>;
