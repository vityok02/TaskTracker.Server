using Application.Abstract.Messaging;

namespace Application.Modules.Members.GetAllMembers;

public sealed record GetAllMembersQuery(Guid UserId, Guid ProjectId)
    : IQuery<IEnumerable<ProjectMemberDto>>;
