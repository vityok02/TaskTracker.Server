using Application.Abstract.Messaging;

namespace Application.Modules.Tags.DeleteTag;

public sealed record DeleteTagCommand(Guid TagId)
    : ICommand;
