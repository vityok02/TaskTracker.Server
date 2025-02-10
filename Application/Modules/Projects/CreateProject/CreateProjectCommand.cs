using Application.Abstract.Messaging;

namespace Application.Modules.Projects.CreateProject;

public sealed record CreateProjectCommand(Guid UserId, string Name, string? Description)
    : ICommand<ProjectResponse>;
