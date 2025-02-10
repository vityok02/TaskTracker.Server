using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Members.AddMember;

internal sealed class AddMemberCommandHandler
    : ICommandHandler<AddMemberCommand, MemberResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IRoleRepository _roleRepository;

    public AddMemberCommandHandler(
        IUserRepository userRepository,
        IProjectMemberRepository projectMemberRepository,
        IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _projectMemberRepository = projectMemberRepository;
        _roleRepository = roleRepository;
    }

    public async Task<Result<MemberResponse>> Handle(
        AddMemberCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetByIdAsync(command.UserId);

        if (user is null)
        {
            return Result<MemberResponse>
                .Failure(UserErrors.NotFound);
        }

        var member = await _projectMemberRepository
            .GetAsync(command.UserId, command.ProjectId);

        if (member is not null)
        {
            return Result<MemberResponse>
                .Failure(ProjectMemberErrors.AlreadyExists);
        }

        var role = await _roleRepository
            .GetByIdAsync(command.RoleId);

        if (role is null)
        {
            return Result<MemberResponse>
                .Failure(RoleErrors.NotFound);
        }

        await _projectMemberRepository
            .CreateMember(command.UserId, command.ProjectId, command.RoleId);

        return new MemberResponse(
            user.Id, command.ProjectId, command.RoleId);
    }
}