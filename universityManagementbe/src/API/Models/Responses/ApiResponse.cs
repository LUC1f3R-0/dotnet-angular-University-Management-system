namespace API.Models.Responses;

public class ApiErrorResponse
{
    public bool Success { get; init; } = false;

    public string Code { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;

    public IDictionary<string, string[]>? Errors { get; init; }

    public string? TraceId { get; init; }
}