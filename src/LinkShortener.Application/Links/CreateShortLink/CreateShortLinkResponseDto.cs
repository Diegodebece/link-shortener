namespace LinkShortener.Application.Links.CreateShortLink;

public sealed record CreateShortLinkResponseDto(
    string ShortCode,
    string ShortUrl,
    string OriginalUrl,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ExpiresAt);
