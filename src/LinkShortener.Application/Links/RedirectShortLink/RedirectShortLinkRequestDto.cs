namespace LinkShortener.Application.Links.RedirectShortLink;

public sealed record RedirectShortLinkRequestDto(
    string ShortCode,
    string? IpAddress = null,
    string? UserAgent = null,
    string? Referrer = null);
