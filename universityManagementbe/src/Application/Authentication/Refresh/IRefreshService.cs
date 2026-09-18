namespace Application.Authentication.Refresh;

public interface IRefreshService
{
    Task<RefreshResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default);
}