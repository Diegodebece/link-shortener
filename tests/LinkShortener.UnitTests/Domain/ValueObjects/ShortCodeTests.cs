using LinkShortener.Domain.Exceptions;
using LinkShortener.Domain.ValueObjects;

namespace LinkShortener.UnitTests.Domain.ValueObjects;

public class ShortCodeTests
{
    [Theory]
    [InlineData("abc12")]
    [InlineData("ABC123")]
    [InlineData("a1B2c3D")]
    public void Create_ShouldReturnShortCode_WhenValueIsValid(string value)
    {
        var shortCode = ShortCode.Create(value);

        Assert.Equal(value, shortCode.Value);
    }

    [Fact]
    public void Create_ShouldTrimValue_WhenValueHasLeadingOrTrailingSpaces()
    {
        var shortCode = ShortCode.Create(" abc12 ");

        Assert.Equal("abc12", shortCode.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldThrowDomainException_WhenValueIsEmpty(string value)
    {
        var act = () => ShortCode.Create(value);

        var exception = Assert.Throws<DomainException>(act);
        Assert.Equal("Short code is required.", exception.Message);
    }

    [Theory]
    [InlineData("abcd")]
    [InlineData("abcdefghijkl3")]
    public void Create_ShouldThrowDomainException_WhenLengthIsInvalid(string value)
    {
        var act = () => ShortCode.Create(value);

        var exception = Assert.Throws<DomainException>(act);
        Assert.Equal($"Short code must be between {ShortCode.MinLength} and {ShortCode.MaxLength} characters.", exception.Message);
    }

    [Theory]
    [InlineData("abc-12")]
    [InlineData("abc_12")]
    [InlineData("abc.12")]
    public void Create_ShouldThrowDomainException_WhenValueContainsNonAlphanumericCharacters(string value)
    {
        var act = () => ShortCode.Create(value);

        var exception = Assert.Throws<DomainException>(act);
        Assert.Equal("Short code can only contain letters and numbers.", exception.Message);
    }
}
