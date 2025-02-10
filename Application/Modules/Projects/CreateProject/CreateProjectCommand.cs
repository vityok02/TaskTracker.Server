using Application.Abstract.Messaging;

namespace Application.Modules.Projects.CreateProject;

public sealed record CreateProjectCommand(Guid UserId, string ProjectName, string? Description)
    : ICommand<ProjectResponse>;
