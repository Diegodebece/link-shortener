using LinkShortener.Application.Common.Exceptions;
using LinkShortener.Application.Common.Interfaces;
using LinkShortener.Domain.Entities;
using LinkShortener.Domain.Repositories;
using LinkShortener.Domain.Services;
using LinkShortener.Domain.ValueObjects;

namespace LinkShortener.Application.Links.CreateShortLink;

public sealed class CreateShortLinkUseCase : ICreateShortLinkUseCase
{
    private const int MaxShortCodeGenerationAttempts = 5;

    private readonly IShortenedLinkRepository _shortenedLinkRepository;
    private readonly IShortCodeGenerator _shortCodeGenerator;
    private readonly IClock _clock;
    private readonly IUnitOfWork _unitOfWork;

    public CreateShortLinkUseCase(
        IShortenedLinkRepository shortenedLinkRepository,
        IShortCodeGenerator shortCodeGenerator,
        IClock clock,
        IUnitOfWork unitOfWork)
    {
        _shortenedLinkRepository = shortenedLinkRepository;
        _shortCodeGenerator = shortCodeGenerator;
        _clock = clock;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateShortLinkResponseDto> ExecuteAsync(
        CreateShortLinkRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var originalUrl = OriginalUrl.Create(request.OriginalUrl);
        var shortCode = await GenerateUniqueShortCodeAsync(cancellationToken);
        var shortenedLink = new ShortenedLink(originalUrl, shortCode, _clock.UtcNow, request.ExpiresAt);

        await _shortenedLinkRepository.AddAsync(shortenedLink, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateShortLinkResponseDto(
            shortCode.Value,
            BuildShortUrl(request.BaseUrl, shortCode),
            originalUrl.Value,
            shortenedLink.CreatedAt,
            shortenedLink.ExpiresAt);
    }

    private async Task<ShortCode> GenerateUniqueShortCodeAsync(CancellationToken cancellationToken)
    {
        for (var attempt = 0; attempt < MaxShortCodeGenerationAttempts; attempt++)
        {
            var shortCode = _shortCodeGenerator.Generate();

            if (!await _shortenedLinkRepository.ExistsByShortCodeAsync(shortCode, cancellationToken))
            {
                return shortCode;
            }
        }

        throw new ShortCodeGenerationException();
    }

    private static string BuildShortUrl(string baseUrl, ShortCode shortCode)
    {
        var normalizedBaseUrl = baseUrl.Trim().TrimEnd('/');

        return $"{normalizedBaseUrl}/{shortCode.Value}";
    }
}
