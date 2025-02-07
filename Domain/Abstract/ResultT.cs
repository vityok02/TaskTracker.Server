namespace Domain.Abstract;

public class Result<TValue> : Result
{
    public TValue Value { get; } = default!;

    private Result(TValue value)
    {
        IsSuccess = true;
        Value = value;
    }

    private Result(Error error)
    {
        IsSuccess = false;
        Error = error;
    }

    private Result(string code, string? description = null)
    {
        IsSuccess = false;
        Error = new Error(code, description);
    }

    public static Result<TValue> Success(TValue value) => new(value);

    public new static Result<TValue> Failure(Error error) => new(error);

    public new static Result<TValue> Failure(string code, string? description = null)
        => new(code, description);

    public static implicit operator Result<TValue>(TValue value) => Success(value);

    public static implicit operator Result<TValue>(Error error) => Failure(error);
}
