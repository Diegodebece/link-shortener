namespace LinkShortener.Application.Links.RedirectShortLink;

public interface IRedirectShortLinkUseCase
{
    Task<RedirectShortLinkResponseDto> ExecuteAsync(
        RedirectShortLinkRequestDto request,
        CancellationToken cancellationToken = default);
}
