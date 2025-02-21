using Application.Abstract.Messaging;

namespace Application.Modules.Members.AddMember;

public sealed record AddMemberCommand(
    Guid UserId, Guid ProjectId, Guid RoleId)
    : ICommand<ProjectMemberDto>;
