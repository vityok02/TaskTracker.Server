using Application.Common.Dtos;
using Application.Modules.Roles;

namespace Application.Modules.Members;

public record ProjectMemberDto(
    UserInfoDto User,
    Guid ProjectId,
    string ProjectName,
    RoleDto Role);