using Application.Abstract.Messaging;

namespace Application.Modules.Tags.ReorderTags;

public sealed record ReorderTagsCommand(
    Guid TagId,
    Guid? BeforeTagId,
    Guid ProjectId,
    Guid UserId)
    : ICommand;
