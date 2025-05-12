namespace Api.Controllers.Tag.Requests;

public record TagRequest(
    string Name,
    string? Color);
