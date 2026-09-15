namespace Application.Authentication.Login;

public interface ILoginService
{
    Task<LoginResult> LoginAsync(string email, string password, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default);
}