using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Projects.DeleteProject;

internal sealed class DeleteProjectCommandHandler
    : ICommandHandler<DeleteProjectCommand>
{
    private readonly IProjectRepository _projectRepository;

    public DeleteProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Result> Handle(
        DeleteProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository
            .GetByIdAsync(command.ProjectId);

        if (project is null)
        {
            return Result
                .Failure(ProjectErrors.NotFound);
        }

        await _projectRepository
            .DeleteAsync(command.ProjectId);

        return Result
            .Success();
    }
}
