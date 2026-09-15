namespace API.Models.Responses;

public sealed class LoginResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public DateTimeOffset AccessTokenExpiresAtUtc { get; init; }
    public string RefreshToken { get; init; } = string.Empty;
    public DateTimeOffset RefreshTokenExpiresAtUtc { get; init; }
    public Guid UserUuid { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}