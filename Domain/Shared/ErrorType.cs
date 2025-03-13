using System.Text.Json.Serialization;

namespace Domain.Shared;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorType
{
    Forbidden,
    ValidationError,
    NotFound,
    Unauthorized,
    InvalidCredentials,
    AlreadyExists,
    InvalidToken,
    InvalidOperation
}