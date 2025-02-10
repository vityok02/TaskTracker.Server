namespace Application.Modules.Members;

public record MemberResponse(
    Guid UserId, Guid ProjectId, Guid RoleId);