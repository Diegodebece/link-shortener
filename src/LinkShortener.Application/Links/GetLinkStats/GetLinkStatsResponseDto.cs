namespace LinkShortener.Application.Links.GetLinkStats;

public sealed record GetLinkStatsResponseDto(
    string ShortCode,
    string OriginalUrl,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ExpiresAt,
    bool IsActive,
    bool IsExpired,
    int TotalClicks,
    DateTimeOffset? LastClickedAt);
