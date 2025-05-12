using Application.Abstract.Messaging;

namespace Application.Modules.Tags.GetAllTags;

public sealed record GetAllTagsQuery(Guid ProjectId)
    : IQuery<IEnumerable<TagDto>>;
