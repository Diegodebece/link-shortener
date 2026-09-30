using LinkShortener.Application.Links.CreateShortLink;
using LinkShortener.Application.Links.GetLinkStats;
using LinkShortener.Application.Links.RedirectShortLink;
using Microsoft.Extensions.DependencyInjection;

namespace LinkShortener.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICreateShortLinkUseCase, CreateShortLinkUseCase>();
            services.AddScoped<IRedirectShortLinkUseCase, RedirectShortLinkUseCase>();
            services.AddScoped<IGetLinkStatsUseCase, GetLinkStatsUseCase>();

            return services;
        }
    }
}