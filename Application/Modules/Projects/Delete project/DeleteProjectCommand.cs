using Application.Abstract.Messaging;

namespace Application.Modules.Projects.Delete_project;

public sealed record DeleteProjectCommand(
    Guid ProjectId)
    : ICommand;
