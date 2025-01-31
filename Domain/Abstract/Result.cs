namespace Domain.Abstract;

public class Result
{
    public bool IsSuccess { get; protected set; }

    public bool IsFailure => !IsSuccess;

    public Error? Error { get; protected set; }

    protected Result()
    {
        IsSuccess = true;
    }

    protected Result(string code, string? description = null)
    {
        IsSuccess = false;
        Error = new Error(code, description);
    }

    protected Result(Error error)
    {
        IsSuccess = false;
        Error = error;
    }

    public static Result Success() => new();

    public static Result Failure(Error error) => new(error);

    public static Result Failure(string code, string? description = null)
        => new(code, description);
}
