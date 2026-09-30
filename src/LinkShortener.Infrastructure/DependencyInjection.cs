using LinkShortener.Application.Common.Interfaces;
using LinkShortener.Domain.Repositories;
using LinkShortener.Domain.Services;
using LinkShortener.Infrastructure.Persistence;
using LinkShortener.Infrastructure.Persistence.Repositories;
using LinkShortener.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LinkShortener.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<LinkShortenerDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IShortenedLinkRepository, ShortenedLinkRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IClock, SystemClock>();
            services.AddSingleton<IShortCodeGenerator, ShortCodeGenerator>();

            return services;
        }

    }
}
