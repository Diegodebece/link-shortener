using LinkShortener.Domain.Exceptions;
using LinkShortener.Domain.ValueObjects;

namespace LinkShortener.UnitTests.Domain.ValueObjects;

public class OriginalUrlTests
{
    [Theory]
    [InlineData("http://example.com")]
    [InlineData("https://example.com/articles/clean-architecture")]
    public void Create_ShouldReturnOriginalUrl_WhenValueIsValid(string value)
    {
        var originalUrl = OriginalUrl.Create(value);

        Assert.Equal(value, originalUrl.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldThrowDomainException_WhenValueIsEmpty(string value)
    {
        var act = () => OriginalUrl.Create(value);

        var exception = Assert.Throws<DomainException>(act);
        Assert.Equal("Original URL is required.", exception.Message);
    }

    [Fact]
    public void Create_ShouldThrowDomainException_WhenValueIsNotAnAbsoluteUrl()
    {
        var act = () => OriginalUrl.Create("not-a-url");

        var exception = Assert.Throws<DomainException>(act);
        Assert.Equal("Original URL must be an absolute URL.", exception.Message);
    }

    [Fact]
    public void Create_ShouldThrowDomainException_WhenSchemeIsNotHttpOrHttps()
    {
        var act = () => OriginalUrl.Create("ftp://example.com");

        var exception = Assert.Throws<DomainException>(act);
        Assert.Equal("Original URL must use HTTP or HTTPS.", exception.Message);
    }

    [Fact]
    public void Create_ShouldThrowDomainException_WhenValueExceedsMaxLength()
    {
        var value = $"https://example.com/{new string('a', OriginalUrl.MaxLength)}";

        var act = () => OriginalUrl.Create(value);

        var exception = Assert.Throws<DomainException>(act);
        Assert.Equal($"Original URL cannot exceed {OriginalUrl.MaxLength} characters.", exception.Message);
    }
}
