namespace Api.Controllers.ProjectMember.Responses;

public record ProjectMemberResponse(
    Guid UserId,
    string UserName,
    Guid ProjectId,
    string ProjectName,
    Guid RoleId,
    string RoleName);
