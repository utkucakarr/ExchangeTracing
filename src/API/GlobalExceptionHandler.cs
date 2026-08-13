using ExchangeTracing.BuildingBlocks.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeTracing.Api;

/// <summary>
/// Translates known exceptions into consistent ProblemDetails responses.
/// Internal exception details are never exposed for unexpected (500) errors.
/// </summary>
internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problem = new ProblemDetails();

        switch (exception)
        {
            case ValidationException validation:
                problem.Status = StatusCodes.Status400BadRequest;
                problem.Title = "Validation failed";
                problem.Extensions["errors"] = validation.Errors
                    .Select(e => new { e.PropertyName, e.ErrorMessage });
                break;

            case ConflictException conflict:
                problem.Status = StatusCodes.Status409Conflict;
                problem.Title = "Conflict";
                problem.Detail = conflict.Message;
                break;

            default:
                logger.LogError(exception, "Unhandled exception");
                problem.Status = StatusCodes.Status500InternalServerError;
                problem.Title = "An unexpected error occurred";
                break;
        }

        httpContext.Response.StatusCode = problem.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}
