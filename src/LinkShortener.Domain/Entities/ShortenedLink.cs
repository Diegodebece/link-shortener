using LinkShortener.Domain.Exceptions;
using LinkShortener.Domain.ValueObjects;

namespace LinkShortener.Domain.Entities;

public sealed class ShortenedLink
{
    private readonly List<LinkClick> _clicks = [];

    private ShortenedLink()
    {
        OriginalUrl = null!;
        ShortCode = null!;
    }

    public ShortenedLink(OriginalUrl originalUrl, ShortCode shortCode, DateTimeOffset createdAt, DateTimeOffset? expiresAt = null)
    {
        if (expiresAt is not null && expiresAt <= createdAt)
        {
            throw new DomainException("Expiration date must be after creation date.");
        }

        OriginalUrl = originalUrl;
        ShortCode = shortCode;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
        IsActive = true;
    }

    public long Id { get; private set; }

    public OriginalUrl OriginalUrl { get; private set; }

    public ShortCode ShortCode { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? ExpiresAt { get; private set; }

    public bool IsActive { get; private set; }

    public int ClickCount { get; private set; }

    public IReadOnlyCollection<LinkClick> Clicks => _clicks.AsReadOnly();

    public bool IsExpired(DateTimeOffset now) => ExpiresAt is not null && ExpiresAt <= now;

    public bool CanRedirect(DateTimeOffset now) => IsActive && !IsExpired(now);

    public void Deactivate()
    {
        IsActive = false;
    }

    public LinkClick RegisterClick(DateTimeOffset clickedAt, string? ipAddress = null, string? userAgent = null, string? referrer = null)
    {
        if (!CanRedirect(clickedAt))
        {
            throw new DomainException("Cannot register a click for an inactive or expired link.");
        }

        var click = new LinkClick(Id, clickedAt, ipAddress, userAgent, referrer);

        _clicks.Add(click);
        ClickCount++;

        return click;
    }
}
