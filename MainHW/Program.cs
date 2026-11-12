using Microsoft.AspNetCore.Mvc;
using MainHW.Models;
using MainHW.Services;
using MainHW.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var messagesDataFilePath = builder.Configuration["MessagesDataFile"] ?? "messages.json";
var usersDataFilePath = builder.Configuration["UsersDataFile"] ?? "users.json";

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<MessageRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<OrderRepository>();
        
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderService>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(c =>
{
    c.LicenseKey = "MY_LICENSE_KEY";
}, typeof(Program));

builder.Services.AddScoped<IRepository<User>, UserRepository>();

// TODO налашутвання роботи з кодуванням JSON

var app = builder.Build();

app.UseStaticFiles();

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
