namespace LinkShortener.Application.Links.GetLinkStats;

public interface IGetLinkStatsUseCase
{
    Task<GetLinkStatsResponseDto> ExecuteAsync(
        GetLinkStatsRequestDto request,
        CancellationToken cancellationToken = default);
}
