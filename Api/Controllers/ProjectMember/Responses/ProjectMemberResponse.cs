namespace Api.Controllers.ProjectMember.Responses;

public record ProjectMemberResponse(
    Guid UserId, string Username, string ProjectName, string Role);
