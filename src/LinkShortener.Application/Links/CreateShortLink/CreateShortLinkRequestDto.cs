namespace LinkShortener.Application.Links.CreateShortLink;

public sealed record CreateShortLinkRequestDto(
    string OriginalUrl,
    string BaseUrl,
    DateTimeOffset? ExpiresAt = null);
