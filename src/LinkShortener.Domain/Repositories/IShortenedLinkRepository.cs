using LinkShortener.Domain.Entities;
using LinkShortener.Domain.ValueObjects;

namespace LinkShortener.Domain.Repositories;

public interface IShortenedLinkRepository
{
    Task<bool> ExistsByShortCodeAsync(ShortCode shortCode, CancellationToken cancellationToken = default);

    Task<ShortenedLink?> GetByShortCodeAsync(ShortCode shortCode, CancellationToken cancellationToken = default);

    Task AddAsync(ShortenedLink shortenedLink, CancellationToken cancellationToken = default);
}
