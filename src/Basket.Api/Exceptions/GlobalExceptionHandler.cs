using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger): IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken ct)
    {
        var (statusCode, title) = exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "Validation failed"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
            _ => (StatusCodes.Status500InternalServerError, "Internal server error")
        };

        logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problem = new ProblemDetails
        {
            Status = statusCode, Title = title, Detail = exception.Message
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(problem, ct);

        return true;
    }
}
