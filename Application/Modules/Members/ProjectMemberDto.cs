using Application.Common.Dtos;

namespace Application.Modules.Members;

public record ProjectMemberDto(
    UserInfoDto User,
    Guid ProjectId,
    string ProjectName,
    Guid RoleId,
    string RoleName);