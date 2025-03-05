using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
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
    private readonly IMapper _mapper;

    public AddMemberCommandHandler(
        IUserRepository userRepository,
        IProjectMemberRepository projectMemberRepository,
        IRoleRepository roleRepository,
        IProjectRepository projectRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _projectMemberRepository = projectMemberRepository;
        _roleRepository = roleRepository;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<Result<ProjectMemberDto>> Handle(
        AddMemberCommand command,
        CancellationToken cancellationToken)
    {
        // TODO: refactor code. Many requests to the database.
        var member = await _projectMemberRepository
            .GetAsync(command.UserId, command.ProjectId);

        if (member is not null)
        {
            return Result<ProjectMemberDto>
                .Failure(ProjectMemberErrors.AlreadyExists);
        }

        var user = await _userRepository
            .GetByIdAsync(command.UserId);

        if (user is null)
        {
            return Result<ProjectMemberDto>
                .Failure(UserErrors.NotFound);
        }

        var role = await _roleRepository
            .GetByIdAsync(command.RoleId);

        if (role is null)
        {
            return Result<ProjectMemberDto>
                .Failure(RoleErrors.NotFound);
        }

        var project = await _projectRepository
            .GetExtendedByIdAsync(command.ProjectId);

        if (project is null)
        {
            return Result<ProjectMemberDto>
                .Failure(ProjectErrors.NotFound);
        }

        var projectMember = await _projectMemberRepository
            .CreateAsync(command.UserId, command.ProjectId, command.RoleId);

        return Result<ProjectMemberDto>.Success(
            _mapper.Map<ProjectMemberDto>(projectMember));
    }
}