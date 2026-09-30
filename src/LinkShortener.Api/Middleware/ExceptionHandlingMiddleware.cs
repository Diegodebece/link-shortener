using LinkShortener.Application.Common.Exceptions;
using LinkShortener.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace LinkShortener.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)

        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ShortenedLinkNotFoundException exception)
            {
                await WriteErrorResponseAsync(
                    context,
                    HttpStatusCode.NotFound,
                    "Not Found",
                    exception.Message);
            }
            catch (DomainException exception)
            {
                await WriteErrorResponseAsync(
                    context,
                    HttpStatusCode.BadRequest,
                    "Bad Request",
                    exception.Message);
            }
            catch (UseCaseException exception)
            {
                await WriteErrorResponseAsync(
                    context,
                    HttpStatusCode.BadRequest,
                    "Bad Request",
                    exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An unhandled exception occurred.");

                await WriteErrorResponseAsync(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Internal Server Error",
                    "An unexpected error occurred.");
            }
        }

        private static async Task WriteErrorResponseAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            string title,
            string detail)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = (int)statusCode,
                title,
                detail
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
