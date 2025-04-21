using Api.Common.Responses;
using Api.Controllers.Role.Responses;

namespace Api.Controllers.ProjectMember.Responses;

public record ProjectMemberResponse(
    UserInfoResponse User,
    Guid ProjectId,
    string ProjectName,
    RoleResponse Role);
