using Microsoft.AspNetCore.Diagnostics;

namespace MainHW.Errors;

public class GlobalExeptionHandler(
    ILogger<GlobalExeptionHandler> logger,
    IProblemDetailsService problemDetailsService,
    IWebHostEnvironment environment
    ) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError; // Internal Server Error

        await httpContext.Response.WriteAsJsonAsync(new
        {
            error = "An unexpected error occurred. Please try again later.",
            message = exception.Message,
            stackTrace = environment.IsDevelopment() ? exception.StackTrace : null
        }, cancellationToken);
        return true;
    }
}
