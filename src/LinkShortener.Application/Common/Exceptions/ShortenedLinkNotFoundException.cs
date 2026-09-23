namespace LinkShortener.Application.Common.Exceptions;

public sealed class ShortenedLinkNotFoundException : ApplicationException
{
    public ShortenedLinkNotFoundException(string shortCode)
        : base($"Shortened link '{shortCode}' was not found.")
    {
    }
}
