namespace LinkShortener.Application.Links.CreateShortLink;

public interface ICreateShortLinkUseCase
{
    Task<CreateShortLinkResponseDto> ExecuteAsync(
        CreateShortLinkRequestDto request,
        CancellationToken cancellationToken = default);
}
