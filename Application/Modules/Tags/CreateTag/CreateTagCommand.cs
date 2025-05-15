using Application.Abstract.Messaging;

namespace Application.Modules.Tags.CreateTag;

public sealed record CreateTagCommand(
    string Name,
    string? Color,
    Guid ProjectId,
    Guid UserId)
    : ICommand<TagDto>;
