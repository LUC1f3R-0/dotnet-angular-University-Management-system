namespace API.Models.Responses;

public sealed class CurrentUserResponse
{
    public Guid UserUuid { get; init; }
    public Guid SessionUuid { get; init; }
    public string Role { get; init; } = string.Empty;
}