namespace Application.Authentication.Logout;

public interface ILogoutService
{
    Task LogoutAsync(string? refreshToken, CancellationToken cancellationToken = default);
}