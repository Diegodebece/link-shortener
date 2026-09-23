using LinkShortener.Domain.Exceptions;

namespace LinkShortener.Domain.ValueObjects;

public sealed record ShortCode
{
    public const int MinLength = 5;
    public const int MaxLength = 12;

    public string Value { get; }

    private ShortCode(string value)
    {
        Value = value;
    }

    public static ShortCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Short code is required.");
        }

        var trimmedValue = value.Trim();

        if (trimmedValue.Length is < MinLength or > MaxLength)
        {
            throw new DomainException($"Short code must be between {MinLength} and {MaxLength} characters.");
        }

        if (!trimmedValue.All(char.IsLetterOrDigit))
        {
            throw new DomainException("Short code can only contain letters and numbers.");
        }

        return new ShortCode(trimmedValue);
    }

    public override string ToString() => Value;
}
