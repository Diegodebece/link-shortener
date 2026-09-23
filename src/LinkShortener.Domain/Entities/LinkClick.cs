namespace LinkShortener.Domain.Entities;

public sealed class LinkClick
{
    private LinkClick()
    {
    }

    public LinkClick(long shortenedLinkId, DateTimeOffset clickedAt, string? ipAddress, string? userAgent, string? referrer)
    {
        ShortenedLinkId = shortenedLinkId;
        ClickedAt = clickedAt;
        IpAddress = string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress.Trim();
        UserAgent = string.IsNullOrWhiteSpace(userAgent) ? null : userAgent.Trim();
        Referrer = string.IsNullOrWhiteSpace(referrer) ? null : referrer.Trim();
    }

    public long Id { get; private set; }

    public long ShortenedLinkId { get; private set; }

    public DateTimeOffset ClickedAt { get; private set; }

    public string? IpAddress { get; private set; }

    public string? UserAgent { get; private set; }

    public string? Referrer { get; private set; }
}
