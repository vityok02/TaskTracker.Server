using Application.Abstract.Messaging;

namespace Application.Modules.Projects.DeleteProject;

public sealed record DeleteProjectCommand(
    Guid ProjectId)
    : ICommand;
