using Application.Abstract.Messaging;

namespace Application.Modules.Members.UpdateMember;

public sealed record UpdateMemberCommand(
    Guid ProjectId,
    Guid UserId,
    Guid RoleId,
    Guid UpdatedById)
    : ICommand;
