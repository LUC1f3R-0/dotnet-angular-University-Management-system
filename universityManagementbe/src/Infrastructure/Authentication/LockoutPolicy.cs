using Application.Authentication.Abstractions;
using Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.Authentication;

public sealed class LockoutPolicy : ILockoutPolicy
{
    public int MaxFailedAttempts { get; }
    public TimeSpan LockoutDuration { get; }

    public LockoutPolicy(IOptions<AccountLockoutOptions> options)
    {
        MaxFailedAttempts = options.Value.MaxFailedAttempts;
        LockoutDuration = TimeSpan.FromMinutes(options.Value.LockoutMinutes);
    }
}
