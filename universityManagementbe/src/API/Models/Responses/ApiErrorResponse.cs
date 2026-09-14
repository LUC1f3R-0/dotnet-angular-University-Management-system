namespace API.Models.Responses;

public class ApiResponse<T>
{
    public bool Success { get; init; } = true;

    public string? Message { get; init; }

    public T? Data { get; init; }
}