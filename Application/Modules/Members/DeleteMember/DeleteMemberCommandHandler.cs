using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Members.DeleteMember;

internal sealed class DeleteMemberCommandHandler
    : ICommandHandler<DeleteMemberCommand>
{
    private readonly IProjectMemberRepository _memberRepository;

    public DeleteMemberCommandHandler(IProjectMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result> Handle(
        DeleteMemberCommand command,
        CancellationToken cancellationToken)
    {
        var member = await _memberRepository
            .GetAsync(command.MemberId, command.ProjectId);

        if (member is null)
        {
            return Result
                .Failure(ProjectMemberErrors.NotFound);
        }

        if (member.UserId == command.PerformedByUserId)
        {
            return Result
                .Failure(ProjectMemberErrors.CannotDeleteYourself);
        }

        await _memberRepository
            .DeleteAsync(member);

        return Result.Success();
    }
}
