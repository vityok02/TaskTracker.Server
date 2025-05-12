using Application.Abstract.Messaging;

namespace Application.Modules.Tags.GetTagById;

public sealed record GetTagByIdQuery(Guid TagId)
    : IQuery<TagDto>;
