namespace API.Models.Responses;

public sealed class LoginResponse
{
    public Guid UserUuid { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}
