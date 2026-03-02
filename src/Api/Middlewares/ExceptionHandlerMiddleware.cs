using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.Api.Middlewares;

public class ExceptionHandlerMiddleware(
    ILogger<ExceptionHandlerMiddleware> logger
) : IExceptionHandler {
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    ) {
        logger.LogError("An unexpected exception occured", exception);

        var response = new ProblemDetails {
            Status = StatusCodes.Status500InternalServerError,
            Detail = "An unexpected error occured on the server",
            Title = "Server error",
        };

        httpContext.Response.StatusCode = response.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}
