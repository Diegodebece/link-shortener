using LinkShortener.Domain.ValueObjects;

namespace LinkShortener.Domain.Services;

public interface IShortCodeGenerator
{
    ShortCode Generate();
}
