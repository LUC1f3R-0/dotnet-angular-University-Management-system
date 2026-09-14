using API.Models.Responses;
using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace API.Exceptions;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred.");

        int statusCode;
        string message;

        if (exception is AppException appException)
        {
            statusCode = (int)appException.StatusCode;
            message = appException.Message;
        }
        else
        {
            statusCode = StatusCodes.Status500InternalServerError;
            message = "An unexpected server error occurred.";
        }

        var response = new ApiResponse<object>
        {
            Success = false,
            Message = message,
            Data = null
        };
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}