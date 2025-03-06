namespace Api.Controllers.ProjectMember.Requests;

public sealed record MemberRequest(Guid UserId, Guid RoleId);
