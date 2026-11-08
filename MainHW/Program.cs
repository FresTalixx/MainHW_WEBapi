using Microsoft.AspNetCore.Mvc;
using MainHW.Models;
using MainHW.Services;
using MainHW.Middleware;

var builder = WebApplication.CreateBuilder(args);

var messagesDataFilePath = builder.Configuration["MessagesDataFile"] ?? "messages.json";
var usersDataFilePath = builder.Configuration["UsersDataFile"] ?? "users.json";

builder.Services.AddSingleton<IDataProvider<Message>>(new JsonFileMessageProvider<Message>(messagesDataFilePath));
builder.Services.AddScoped<MessageService>();

builder.Services.AddSingleton<IDataProvider<User>>(new JsonFileMessageProvider<User>(usersDataFilePath));
builder.Services.AddScoped<UserService>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(c =>
{
    c.LicenseKey = "MY_LICENSE_KEY";
}, typeof(Program));

// TODO налашутвання роботи з кодуванням JSON

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "My API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}


app.Use(async (/*HttpContext*/ context, /*Func<Task>*/ next) =>
{
    var logMessage = $"[{DateTime.Now}] {context.Request.Method} {context.Request.Path} ip:{context.Connection.RemoteIpAddress}";
    Console.WriteLine(logMessage);
    
    await next(context);
});

app.UseMiddleware<UserAutorizationByToken>();

app.MapControllers();

app.Run();