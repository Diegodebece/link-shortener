using LinkShortener.Application.Common.Exceptions;
using LinkShortener.Application.Common.Interfaces;
using LinkShortener.Domain.Repositories;
using LinkShortener.Domain.ValueObjects;

namespace LinkShortener.Application.Links.RedirectShortLink;

public sealed class RedirectShortLinkUseCase : IRedirectShortLinkUseCase
{
    private readonly IShortenedLinkRepository _shortenedLinkRepository;
    private readonly IClock _clock;
    private readonly IUnitOfWork _unitOfWork;

    public RedirectShortLinkUseCase(
        IShortenedLinkRepository shortenedLinkRepository,
        IClock clock,
        IUnitOfWork unitOfWork)
    {
        _shortenedLinkRepository = shortenedLinkRepository;
        _clock = clock;
        _unitOfWork = unitOfWork;
    }

    public async Task<RedirectShortLinkResponseDto> ExecuteAsync(
        RedirectShortLinkRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var shortCode = ShortCode.Create(request.ShortCode);
        var shortenedLink = await _shortenedLinkRepository.GetByShortCodeAsync(shortCode, cancellationToken);

        if (shortenedLink is null)
        {
            throw new ShortenedLinkNotFoundException(shortCode.Value);
        }

        shortenedLink.RegisterClick(_clock.UtcNow, request.IpAddress, request.UserAgent, request.Referrer);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RedirectShortLinkResponseDto(shortenedLink.OriginalUrl.Value);
    }
}
