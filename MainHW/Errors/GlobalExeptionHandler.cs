using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

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

        if (httpContext.Response.HasStarted)
        {
            return false;
        }

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred.",
            Detail = environment.IsDevelopment()
                ? exception.Message
                : "Please try again later.",
            Instance = httpContext.Request.Path
        };

        var context = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problemDetails
        };

        return await problemDetailsService.TryWriteAsync(context);
    }
}
