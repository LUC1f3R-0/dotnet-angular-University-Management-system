namespace API.Models.Responses;

public sealed class ApiErrorResponse
{
    public bool Success { get; init; } = false;
    public string? Message { get; init; }
}
