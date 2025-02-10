namespace Domain.Shared;

public class Result
{
    public bool IsSuccess { get; protected set; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; protected set; } = Error.None;

    protected Result()
    {
        IsSuccess = true;
    }

    protected Result(string code, string message)
    {
        IsSuccess = false;
        Error = new Error(code, message);
    }

    protected Result(Error error)
    {
        IsSuccess = false;
        Error = error;
    }

    public static Result Success() => new();

    public static Result Failure(Error error) => new(error);

    public static Result Failure(string code, string message)
        => new(code, message);
}
