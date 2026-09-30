namespace LinkShortener.Application.Common.Exceptions;

public sealed class ShortCodeGenerationException : UseCaseException
{
    public ShortCodeGenerationException()
        : base("A unique short code could not be generated.")
    {
    }
}
