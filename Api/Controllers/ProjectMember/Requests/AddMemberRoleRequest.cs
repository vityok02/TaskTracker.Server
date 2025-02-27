namespace Api.Controllers.ProjectMember.Requests;

public sealed record AddMemberRoleRequest(Guid UserId, Guid RoleId);
