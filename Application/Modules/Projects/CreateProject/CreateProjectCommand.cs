using Application.Abstract.Messaging;

namespace Application.Modules.Projects.CreateProject;

public sealed record CreateProjectCommand(Guid UserId, ProjectRequest Project)
    : ICommand<ProjectResponse>;
