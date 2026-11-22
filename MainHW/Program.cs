using Microsoft.AspNetCore.Mvc;
using MainHW.Models;
using MainHW.Services;
using MainHW.Middleware;
using Microsoft.EntityFrameworkCore;
using MainHW.Repositories;
using FluentValidation;
using MainHW.Models.Validation;
using MainHW.Errors;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<MessageRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<CategoryRepository>();

builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<UploadFileService>();

builder.Services.AddControllers();

builder.Services.AddControllers(o =>
{
    // o.Filters.Add<FluentValidationActionFilter>();
})
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExeptionHandler>();

builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(c =>
{
    c.LicenseKey = "MY_LICENSE_KEY";
}, typeof(Program));

builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<UserRegisterValidator>();

// TODO налашутвання роботи з кодуванням JSON

var app = builder.Build();

app.UseExceptionHandler();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "My API V1");
        c.RoutePrefix = "swagger";
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
