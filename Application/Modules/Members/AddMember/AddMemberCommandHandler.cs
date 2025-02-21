using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Members.AddMember;

internal sealed class AddMemberCommandHandler
    : ICommandHandler<AddMemberCommand, ProjectMemberDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IProjectRepository _projectRepository;

    public AddMemberCommandHandler(
        IUserRepository userRepository,
        IProjectMemberRepository projectMemberRepository,
        IRoleRepository roleRepository,
        IProjectRepository projectRepository)
    {
        _userRepository = userRepository;
        _projectMemberRepository = projectMemberRepository;
        _roleRepository = roleRepository;
        _projectRepository = projectRepository;
    }

    public async Task<Result<ProjectMemberDto>> Handle(
        AddMemberCommand command,
        CancellationToken cancellationToken)
    {
        // TODO: refactor code. Many requests to the database.

        var user = await _userRepository
            .GetByIdAsync(command.UserId);

        if (user is null)
        {
            return Result<ProjectMemberDto>
                .Failure(UserErrors.NotFound);
        }

        var member = await _projectMemberRepository
            .GetAsync(command.UserId, command.ProjectId);

        if (member is not null)
        {
            return Result<ProjectMemberDto>
                .Failure(ProjectMemberErrors.AlreadyExists);
        }

        var role = await _roleRepository
            .GetByIdAsync(command.RoleId);

        if (role is null)
        {
            return Result<ProjectMemberDto>
                .Failure(RoleErrors.NotFound);
        }

        var project = await _projectRepository
            .GetByIdAsync(user.Id, command.ProjectId);

        if (project is null)
        {
            return Result<ProjectMemberDto>
                .Failure(ProjectErrors.NotFound);
        }

        await _projectMemberRepository
            .CreateMember(command.UserId, command.ProjectId, command.RoleId);

        return new ProjectMemberDto(
            user.Id, user.UserName, project.Name, role.Name);
    }
}