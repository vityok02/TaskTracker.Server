using Application.Abstract.Messaging;

namespace Application.Projects.CreateProject;

public sealed record CreateProjectCommand(Guid UserId, ProjectRequest Project) : ICommand<ProjectResponse>;
