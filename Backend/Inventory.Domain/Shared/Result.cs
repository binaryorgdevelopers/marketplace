namespace Inventory.Domain.Shared;

public class Result
{
    public bool IsSuccess { get; set; }
    public bool IsFailure { get; set; }
    public Error Error { get; set; }

    public Result(bool isSuccess, Error error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
        IsFailure = !isSuccess;
    }

    public static Result Success()
    {
        return new Result(true);
    }

    public static Result<T> Success<T>(T data)
    {
        return new Result<T>(true, data, null);
    }

    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }

    public static Result<T> Failure<T>(Error error) where T : class
    {
        return new Result<T>(false, null as T, error);
    }

    public static Result<T> Create<T>(T value)
    {
        return new Result<T>(true, value, null);
    }
}

public class Result<T> : Result
{
    public T Value { get; }

    public Result(bool isSuccess, T value, Error error = null)
        : base(isSuccess, error)
    {
        Value = value;
    }
}