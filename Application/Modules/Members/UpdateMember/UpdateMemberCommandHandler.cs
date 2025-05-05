using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Members.UpdateMember;

internal sealed class UpdateMemberCommandHandler
    : ICommandHandler<UpdateMemberCommand>
{
    private readonly IProjectMemberRepository _projectMemberRepository;

    public UpdateMemberCommandHandler(IProjectMemberRepository projectMemberRepository)
    {
        _projectMemberRepository = projectMemberRepository;
    }

    public async Task<Result> Handle(
        UpdateMemberCommand command,
        CancellationToken cancellationToken)
    {
        var member = await _projectMemberRepository
            .GetAsync(command.UserId, command.ProjectId);

        if (member is null)
        {
            return Result
                .Failure(ProjectMemberErrors.NotFound);
        }

        if (member.UserId == command.UpdatedById)
        {
            return Result
                .Failure(ProjectMemberErrors.CannotUpdateYourself);
        }

        member.RoleId = command.RoleId;

        await _projectMemberRepository
            .UpdateAsync(member);

        return Result.Success();
    }
}