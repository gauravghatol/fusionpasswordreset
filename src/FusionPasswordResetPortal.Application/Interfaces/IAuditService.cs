using FusionPasswordResetPortal.Domain.Enums;

namespace FusionPasswordResetPortal.Application.Interfaces;

public interface IAuditService
{
    Task LogEventAsync(
        AuditEventType eventType,
        string status,
        string targetUserName,
        string oracleInstanceName,
        bool isAuthorized,
        bool isSuccessful,
        string? detail = null,
        CancellationToken cancellationToken = default);
}
