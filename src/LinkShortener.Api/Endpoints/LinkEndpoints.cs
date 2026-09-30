using LinkShortener.Application.Links.CreateShortLink;
using LinkShortener.Application.Links.RedirectShortLink;
using LinkShortener.Application.Links.GetLinkStats;

namespace LinkShortener.Api.Endpoints
{
    public static class LinkEndpoints
    {
        public static IEndpointRouteBuilder MapLinkEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/links", CreateShortLinkAsync)
                .WithName("CreateShortLink");

            app.MapGet("/{shortCode:regex(^[a-zA-Z0-9]{{5,12}}$)}", RedirectShortLinkAsync)
                .WithName("RedirectShortLink");

            app.MapGet("/api/links/{shortCode}/stats", GetLinkStatsAsync)
                .WithName("GetLinkStats");

            return app;
        }

        private static async Task<IResult> CreateShortLinkAsync(
            CreateShortLinkApiRequest request,
            HttpContext httpContext,
            ICreateShortLinkUseCase useCase,
            CancellationToken cancellationToken)
        {
            var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
            var useCaseRequest = new CreateShortLinkRequestDto(
                request.OriginalUrl,
                baseUrl,
                request.ExpiresAt);

            var response = await useCase.ExecuteAsync(useCaseRequest, cancellationToken);

            return Results.Created(response.ShortUrl, response);
        }

        private static async Task<IResult> RedirectShortLinkAsync(
            string shortCode,
            HttpContext httpContext,
            IRedirectShortLinkUseCase useCase,
            CancellationToken cancellationToken)
        {
            var request = new RedirectShortLinkRequestDto(
                shortCode,
                httpContext.Connection.RemoteIpAddress?.ToString(),
                httpContext.Request.Headers.UserAgent.ToString(),
                httpContext.Request.Headers.Referer.ToString());

            var response = await useCase.ExecuteAsync(request, cancellationToken);

            return Results.Redirect(response.OriginalUrl);
        }

        private static async Task<IResult> GetLinkStatsAsync(
            string shortCode,
            IGetLinkStatsUseCase useCase,
            CancellationToken cancellationToken)
        {
            var request = new GetLinkStatsRequestDto(shortCode);
            var response = await useCase.ExecuteAsync(request, cancellationToken);

            return Results.Ok(response);
        }

    }
}
