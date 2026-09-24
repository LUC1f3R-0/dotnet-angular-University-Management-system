namespace Application.Authentication.Abstractions;

public interface ILockoutPolicy
{
    int MaxFailedAttempts { get; }
    TimeSpan LockoutDuration { get; }
}
