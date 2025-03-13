using Application.Abstract.Messaging;

namespace Application.Modules.Members.DeleteMember;

public sealed record DeleteMemberCommand(
    Guid ProjectId,
    Guid MemberId,
    Guid PerformedByUserId)
    : ICommand;
