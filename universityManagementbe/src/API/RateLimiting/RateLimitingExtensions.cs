using System.Threading.RateLimiting;
using Infrastructure.Options;
using Microsoft.AspNetCore.RateLimiting;

namespace API.RateLimiting;

public static class RateLimitingExtensions
{
    public const string LoginPolicy = "LoginPolicy";
    public const string RefreshPolicy = "RefreshPolicy";

    public static IServiceCollection AddApiRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(RateLimitingOptions.SectionName).Get<RateLimitingOptions>() ?? new RateLimitingOptions();

        services.AddRateLimiter(limiterOptions =>
        {
            limiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            limiterOptions.AddPolicy(LoginPolicy, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = options.LoginPermitLimit,
                        Window = TimeSpan.FromSeconds(options.LoginWindowSeconds),
                        QueueLimit = 0
                    }));

            limiterOptions.AddPolicy(RefreshPolicy, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = options.RefreshPermitLimit,
                        Window = TimeSpan.FromSeconds(options.RefreshWindowSeconds),
                        QueueLimit = 0
                    }));
        });

        return services;
    }
}
