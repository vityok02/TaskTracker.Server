using Api.Common;

namespace Api.Controllers.ProjectMember.Responses;

public record ProjectMemberResponse(
    UserInfoResponse User,
    Guid ProjectId,
    string ProjectName,
    Guid RoleId,
    string RoleName);
