using LinkShortener.Application.Common.Exceptions;
using LinkShortener.Application.Common.Interfaces;
using LinkShortener.Domain.Repositories;
using LinkShortener.Domain.ValueObjects;

namespace LinkShortener.Application.Links.GetLinkStats;

public sealed class GetLinkStatsUseCase : IGetLinkStatsUseCase
{
    private readonly IShortenedLinkRepository _shortenedLinkRepository;
    private readonly IClock _clock;

    public GetLinkStatsUseCase(
        IShortenedLinkRepository shortenedLinkRepository,
        IClock clock)
    {
        _shortenedLinkRepository = shortenedLinkRepository;
        _clock = clock;
    }

    public async Task<GetLinkStatsResponseDto> ExecuteAsync(
        GetLinkStatsRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var shortCode = ShortCode.Create(request.ShortCode);
        var shortenedLink = await _shortenedLinkRepository.GetByShortCodeAsync(shortCode, cancellationToken);

        if (shortenedLink is null)
        {
            throw new ShortenedLinkNotFoundException(shortCode.Value);
        }

        var lastClickedAt = shortenedLink.Clicks
            .OrderByDescending(click => click.ClickedAt)
            .Select(click => (DateTimeOffset?)click.ClickedAt)
            .FirstOrDefault();

        return new GetLinkStatsResponseDto(
            shortenedLink.ShortCode.Value,
            shortenedLink.OriginalUrl.Value,
            shortenedLink.CreatedAt,
            shortenedLink.ExpiresAt,
            shortenedLink.IsActive,
            shortenedLink.IsExpired(_clock.UtcNow),
            shortenedLink.ClickCount,
            lastClickedAt);
    }
}
