using System.Text.Json;
using MainHW.Models;
using MainHW.Services;

namespace MainHW.Middleware;

public class UserAutorizationByToken
{
    private readonly RequestDelegate _next;

    public UserAutorizationByToken(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, UserService userService)
    {
        var token = GetToken(context.Request);
        if (string.IsNullOrWhiteSpace(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Authorization token is missing.");
            return;
        }

        var user = await userService.GetUserByTokenAsync(token);
        if (user == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid authorization token.");
            return;
        }

        context.Request.EnableBuffering();
        var request = await JsonSerializer.DeserializeAsync<Message>(context.Request.Body);
        context.Request.Body.Position = 0;

        if (request == null || !user.Email.Equals(request.From, StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("The sender email must match the email associated with the authorization token.");
            return;
        }

        await _next(context);
    }

    private static string? GetToken(HttpRequest request)
    {
        var customHeader = request.Headers["X-My-Authorization"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(customHeader))
        {
            return customHeader;
        }

        var authorizationHeader = request.Headers.Authorization.FirstOrDefault();
        if (authorizationHeader?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true)
        {
            return authorizationHeader["Bearer ".Length..].Trim();
        }

        return null;
    }

}
