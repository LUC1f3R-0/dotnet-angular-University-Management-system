namespace Domain.Enums;

public enum SecurityEventType
{
    LoginSucceeded = 1,
    LoginFailed = 2,
    Logout = 3,
    EmailVerified = 4,
    OtpFailed = 5,
    PasswordChanged = 6,
    PasswordReset = 7,
    AccountLocked = 8,
    SessionRevoked = 9
}