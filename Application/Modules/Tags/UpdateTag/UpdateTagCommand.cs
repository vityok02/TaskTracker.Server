using Application.Abstract.Messaging;

namespace Application.Modules.Tags.UpdateTag;

public sealed record UpdateTagCommand(
    Guid TagId,
    string Name,
    string? Color,
    Guid UserId)
    : ICommand<TagDto>;
