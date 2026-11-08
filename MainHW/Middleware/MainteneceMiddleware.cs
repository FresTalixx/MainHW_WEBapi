namespace MainHW.Middleware;

public class MainteneceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    public MainteneceMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var maintenanceMode = _configuration.GetValue<bool>("Maintenance");

        if (maintenanceMode)
        {
            context.Response.StatusCode = 503;
            context.Response.ContentType = "text/html";

            await context.Response.WriteAsync(
                "<html><body><h1>Server is under maintenance. Please try again later.</h1></body></html>"
            );

            return;
        }

        await _next(context);
    }
}
