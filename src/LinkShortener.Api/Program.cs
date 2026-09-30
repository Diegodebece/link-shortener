using LinkShortener.Infrastructure;
using LinkShortener.Application;
using LinkShortener.Api.Endpoints;
using LinkShortener.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        status = "Healthy",
        timestamp = DateTimeOffset.UtcNow
    });
})
.WithName("HealthCheck");

app.MapLinkEndpoints();

app.Run();


