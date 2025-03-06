using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Members.DeleteMember;

public sealed record DeleteMemberCommand(
    Guid ProjectId,
    Guid UserId)
    : ICommand;

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
            .GetAsync(command.UserId, command.ProjectId);

        if (member is null)
        {
            return Result
                .Failure(ProjectMemberErrors.NotFound);
        }

        await _memberRepository
            .DeleteAsync(member);

        return Result.Success();
    }
}
