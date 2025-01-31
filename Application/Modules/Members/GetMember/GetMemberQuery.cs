using Application.Abstract.Messaging;

namespace Application.Modules.Members.GetMember;

public sealed record GetMemberQuery(Guid UserId, Guid ProjectId)
    : IQuery<MemberResponse>;
