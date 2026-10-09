using LinkShortener.Infrastructure;
using LinkShortener.Application;
using LinkShortener.Api.Endpoints;
using LinkShortener.Api.Middleware;
using Microsoft.AspNetCore.HttpOverrides;

const string FrontendCorsPolicy = "Frontend";

var builder = WebApplication.CreateBuilder(args);

// Comma-separated list, e.g. "https://my-app.vercel.app,http://localhost:5500".
// On Render set the env var: Cors__AllowedOrigins
var allowedOrigins = (builder.Configuration["Cors:AllowedOrigins"] ?? string.Empty)
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .Select(origin => origin.TrimEnd('/'))
    .ToArray();

builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontendCorsPolicy, policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .WithMethods("GET", "POST")
            .WithHeaders("Content-Type");
    });
});

var app = builder.Build();
app.UseForwardedHeaders();

// Before the exception middleware so error responses also carry CORS headers
// and the frontend can read their "detail" message.
app.UseCors(FrontendCorsPolicy);

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseHttpsRedirection();

}

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


