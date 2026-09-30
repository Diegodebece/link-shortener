namespace LinkShortener.Application.Common.Exceptions;

public sealed class ShortenedLinkNotFoundException : UseCaseException
{
    public ShortenedLinkNotFoundException(string shortCode)
        : base($"Shortened link '{shortCode}' was not found.")
    {
    }
}
