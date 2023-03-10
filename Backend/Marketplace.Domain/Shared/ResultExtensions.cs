namespace Marketplace.Domain.Shared;

public static class ResultExtensions
{
    public static Result<T> Ensure<T>(
        this Result<T> result,
        Func<T, bool> predicate,
        Error error) where T : class
    {
        if (result.IsFailure)
        {
            return result;
        }

        return predicate(result.Value) ? result : Result.Failure<T>(error);
    }

    public static Result<TOut> Map<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> mappingFunc) where TOut : class =>
        result.IsSuccess ? Result.Success(mappingFunc(result.Value)) : Result.Failure<TOut>(result.Error);

    public static async Task<Result> Bind<TIn>(
        this Result<TIn> result,
        Func<TIn, Task<Result>> func)
    {
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        return await func(result.Value);
    }

    public static async Task<Result> Bind<TIn>(
        this Result<TIn> result,
        Func<TIn, Task<Result<TIn>>> func) where TIn : class
    {
        if (result.IsFailure)
        {
            return Result.Failure<TIn>(result.Error);
        }

        return await func(result.Value);
    }

    public static async Task<Result<TOut>> Bind<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Task<Result<TOut>>> func) where TOut : class

    {
        if (result.IsFailure)
        {
            return Result.Failure<TOut>(result.Error);
        }

        return await func(result.Value);
    }
}