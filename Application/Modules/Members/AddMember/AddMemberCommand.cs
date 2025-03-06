using Application.Abstract.Messaging;

namespace Application.Modules.Members.AddMember;

public sealed record AddMemberCommand(
    Guid ProjectId, Guid UserId, Guid RoleId)
    : ICommand<ProjectMemberDto>;
