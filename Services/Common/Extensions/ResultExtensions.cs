using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Shared.Extensions;

public static class ResultExtensions
{
    public static async Task<IActionResult> Match(
        this Task<Result> resultTask,
        Func<IActionResult> onSuccess,
        Func<Result, IActionResult> onFailure)
    {
        Result result = await resultTask;
        return result.IsSuccess ? onSuccess() : onFailure(result);
    }

    public static async Task<IActionResult> Match<TIn>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, IActionResult> onSuccess,
        Func<Result, IActionResult> onFailure)
    {
        Result<TIn> result = await resultTask;
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
    }

    public static async ValueTask<IActionResult> Match(
        this ValueTask<Result> resultTask,
        Func<IActionResult> onSuccess,
        Func<Result, IActionResult> onFailure)
    {
        Result result = await resultTask;
        return result.IsSuccess ? onSuccess() : onFailure(result);
    }

    public static async ValueTask<IActionResult> Match<TIn>(
        this ValueTask<Result<TIn>> resultTask,
        Func<TIn, IActionResult> onSuccess,
        Func<Result, IActionResult> onFailure)
    {
        Result<TIn> result = await resultTask;
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
    }
}