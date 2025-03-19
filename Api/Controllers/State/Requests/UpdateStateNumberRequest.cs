namespace Api.Controllers.State.Requests;

public record UpdateStateNumberRequest(
    Guid StateId1,
    Guid StateId2
);
