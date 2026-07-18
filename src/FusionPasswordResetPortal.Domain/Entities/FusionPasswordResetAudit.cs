using FusionPasswordResetPortal.Domain.Common;
using FusionPasswordResetPortal.Domain.Enums;

namespace FusionPasswordResetPortal.Domain.Entities;

public class FusionPasswordResetAudit : BaseEntity
{
    public string RequesterEmail { get; set; } = string.Empty;
    public string OracleInstanceName { get; set; } = string.Empty;
    public string TargetUserName { get; set; } = string.Empty;
    public AuditEventType EventType { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? CorrelationId { get; set; }
    public string? Detail { get; set; }
    public bool IsAuthorized { get; set; }
    public bool IsSuccessful { get; set; }
    public DateTime ActionedOnUtc { get; set; } = DateTime.UtcNow;
}
