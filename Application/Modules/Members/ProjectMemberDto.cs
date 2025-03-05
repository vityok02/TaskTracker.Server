namespace Application.Modules.Members;

public record ProjectMemberDto(
    Guid UserId,
    string UserName,
    Guid ProjectId,
    string ProjectName,
    Guid RoleId,
    string RoleName);