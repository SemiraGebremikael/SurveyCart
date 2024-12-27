namespace SurveyCart.Api.Abstractions;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure { get; }
    public Error Error { get; } = default!;
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;

        if ((isSuccess && Error != Error.None) || !isSuccess && Error == Error.None)
        {
            throw new InvalidOperationException();
        }
    }

    public static Result Success()
    {
        return new Result(true, Error.None);
    }

    public static Result Failure(Error error)
    {
        return new Result(false, Error.None);
    }
  
}
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }
    public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("Failure results cannot have value");

}
