namespace Api.Controllers.ProjectMember.Requests;

public sealed record RoleRequest(Guid UserId, Guid RoleId);
