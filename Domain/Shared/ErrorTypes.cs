namespace Domain.Shared;

public static class ErrorTypes
{
    public const string ValidationError = "ValidationError";
    public const string NotFound = "NotFound";
    public const string Unauthorized = "Unauthorized";
    public const string InvalidCredentials = "InvalidCredentials";
    public const string Conflict = "AlreadyExists";
}
