using LinkShortener.Domain.Entities;
using LinkShortener.Domain.Exceptions;
using LinkShortener.Domain.ValueObjects;

namespace LinkShortener.UnitTests.Domain.Entities;

public class ShortenedLinkTests
{
    [Fact]
    public void Constructor_ShouldCreateActiveLink_WhenValuesAreValid()
    {
        var createdAt = DateTimeOffset.UtcNow;

        var shortenedLink = CreateShortenedLink(createdAt);

        Assert.Equal("https://example.com", shortenedLink.OriginalUrl.Value);
        Assert.Equal("abc12", shortenedLink.ShortCode.Value);
        Assert.Equal(createdAt, shortenedLink.CreatedAt);
        Assert.Null(shortenedLink.ExpiresAt);
        Assert.True(shortenedLink.IsActive);
        Assert.Equal(0, shortenedLink.ClickCount);
        Assert.Empty(shortenedLink.Clicks);
    }

    [Fact]
    public void Constructor_ShouldThrowDomainException_WhenExpirationDateIsBeforeCreationDate()
    {
        var createdAt = DateTimeOffset.UtcNow;
        var expiresAt = createdAt.AddMinutes(-1);

        var act = () => CreateShortenedLink(createdAt, expiresAt);

        var exception = Assert.Throws<DomainException>(act);
        Assert.Equal("Expiration date must be after creation date.", exception.Message);
    }

    [Fact]
    public void IsExpired_ShouldReturnTrue_WhenExpirationDateHasPassed()
    {
        var createdAt = DateTimeOffset.UtcNow;
        var expiresAt = createdAt.AddMinutes(10);
        var shortenedLink = CreateShortenedLink(createdAt, expiresAt);

        var isExpired = shortenedLink.IsExpired(expiresAt.AddSeconds(1));

        Assert.True(isExpired);
    }

    [Fact]
    public void CanRedirect_ShouldReturnFalse_WhenLinkIsInactive()
    {
        var shortenedLink = CreateShortenedLink(DateTimeOffset.UtcNow);

        shortenedLink.Deactivate();

        Assert.False(shortenedLink.CanRedirect(DateTimeOffset.UtcNow));
    }

    [Fact]
    public void RegisterClick_ShouldAddClickAndIncrementClickCount_WhenLinkCanRedirect()
    {
        var clickedAt = DateTimeOffset.UtcNow;
        var shortenedLink = CreateShortenedLink(clickedAt.AddMinutes(-1));

        var click = shortenedLink.RegisterClick(
            clickedAt,
            "127.0.0.1",
            "UnitTestBrowser",
            "https://referrer.com");

        Assert.Equal(1, shortenedLink.ClickCount);
        Assert.Single(shortenedLink.Clicks);
        Assert.Same(click, shortenedLink.Clicks.Single());
        Assert.Equal(clickedAt, click.ClickedAt);
        Assert.Equal("127.0.0.1", click.IpAddress);
        Assert.Equal("UnitTestBrowser", click.UserAgent);
        Assert.Equal("https://referrer.com", click.Referrer);
    }

    [Fact]
    public void RegisterClick_ShouldThrowDomainException_WhenLinkIsExpired()
    {
        var createdAt = DateTimeOffset.UtcNow;
        var expiresAt = createdAt.AddMinutes(10);
        var shortenedLink = CreateShortenedLink(createdAt, expiresAt);

        var act = () => shortenedLink.RegisterClick(expiresAt.AddSeconds(1));

        var exception = Assert.Throws<DomainException>(act);
        Assert.Equal("Cannot register a click for an inactive or expired link.", exception.Message);
    }

    private static ShortenedLink CreateShortenedLink(
        DateTimeOffset createdAt,
        DateTimeOffset? expiresAt = null)
    {
        return new ShortenedLink(
            OriginalUrl.Create("https://example.com"),
            ShortCode.Create("abc12"),
            createdAt,
            expiresAt);
    }
}
