using LinkShortener.Application.Common.Interfaces;

namespace LinkShortener.Infrastructure.Services
{
    public class SystemClock : IClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
