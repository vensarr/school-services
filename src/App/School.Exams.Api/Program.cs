using School.Exams.Api.Filters;
using School.Exams.Api.Middleware;
using School.Exams.Application;
using School.Exams.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "School Exams API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseMiddleware<ValidationExceptionMiddleware>();

app.MapControllers();

app.Run();
