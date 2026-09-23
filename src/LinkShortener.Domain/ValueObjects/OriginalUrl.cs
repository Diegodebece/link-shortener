using LinkShortener.Domain.Exceptions;

namespace LinkShortener.Domain.ValueObjects;

public sealed record OriginalUrl
{
    public const int MaxLength = 2048;

    public string Value { get; }

    private OriginalUrl(string value)
    {
        Value = value;
    }

    public static OriginalUrl Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Original URL is required.");
        }

        var trimmedValue = value.Trim();

        if (trimmedValue.Length > MaxLength)
        {
            throw new DomainException($"Original URL cannot exceed {MaxLength} characters.");
        }

        if (!Uri.TryCreate(trimmedValue, UriKind.Absolute, out var uri))
        {
            throw new DomainException("Original URL must be an absolute URL.");
        }

        if (uri.Scheme is not "http" and not "https")
        {
            throw new DomainException("Original URL must use HTTP or HTTPS.");
        }

        if (string.IsNullOrWhiteSpace(uri.Host))
        {
            throw new DomainException("Original URL must include a valid host.");
        }

        return new OriginalUrl(trimmedValue);
    }

    public override string ToString() => Value;
}
