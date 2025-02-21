namespace Api.Controllers.ProjectMember.Requests.Responses;

public record ProjectMemberResponse(
    Guid UserId, string Username, string ProjectName, string Role);
