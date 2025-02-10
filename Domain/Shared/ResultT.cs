namespace Domain.Shared;

public class Result<TValue> : Result
{
    public TValue Value { get; } = default!;

    protected Result(TValue value)
    {
        IsSuccess = true;
        Value = value;
    }

    protected Result(Error error)
    {
        IsSuccess = false;
        Error = error;
    }

    protected Result(string code, string message)
    {
        IsSuccess = false;
        Error = new Error(code, message);
    }

    public static Result<TValue> Success(TValue value) => new(value);

    public new static Result<TValue> Failure(Error error) => new(error);

    public new static Result<TValue> Failure(string code, string message)
        => new(code, message);

    public static implicit operator Result<TValue>(TValue value) => Success(value);

    public static implicit operator Result<TValue>(Error error) => Failure(error);
}
