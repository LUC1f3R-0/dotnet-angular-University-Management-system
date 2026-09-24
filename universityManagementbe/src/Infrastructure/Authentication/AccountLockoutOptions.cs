namespace Infrastructure.Authentication;

public sealed class AccountLockoutOptions
{
    public const string SectionName = "Lockout";

    public int MaxFailedAttempts { get; set; } = 5;
    public int LockoutMinutes { get; set; } = 15;
}
