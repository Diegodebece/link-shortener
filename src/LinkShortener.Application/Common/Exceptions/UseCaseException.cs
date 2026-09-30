namespace LinkShortener.Application.Common.Exceptions;

public class UseCaseException : Exception
{
    public UseCaseException(string message)
        : base(message)
    {
    }
}
