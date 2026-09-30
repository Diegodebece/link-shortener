namespace LinkShortener.Api
{
    public sealed record CreateShortLinkApiRequest(
        string OriginalUrl,
        DateTimeOffset? ExpiresAt = null);
        
}
