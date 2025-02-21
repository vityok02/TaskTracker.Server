namespace Application.Modules.Members;

public record ProjectMemberDto(
    Guid UserId, string Username, string ProjectName, string Role);