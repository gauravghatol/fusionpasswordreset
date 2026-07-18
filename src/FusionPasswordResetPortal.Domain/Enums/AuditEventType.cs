namespace FusionPasswordResetPortal.Domain.Enums;

public enum AuditEventType
{
    LoginAttempt = 1,
    UserSearch = 2,
    PasswordReset = 3,
    FailedApiCall = 4,
    SecurityEvent = 5
}
