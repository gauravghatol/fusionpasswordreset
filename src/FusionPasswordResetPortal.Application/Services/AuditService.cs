using FusionPasswordResetPortal.Application.Interfaces;
using FusionPasswordResetPortal.Domain.Entities;
using FusionPasswordResetPortal.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace FusionPasswordResetPortal.Application.Services;

public class AuditService : IAuditService
{
    private readonly IAuditRepository _auditRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;
    private readonly ILogger<AuditService> _logger;

    public AuditService(
        IAuditRepository auditRepository,
        IUnitOfWork unitOfWork,
        IUserContextService userContextService,
        ILogger<AuditService> logger)
    {
        _auditRepository = auditRepository;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
        _logger = logger;
    }

    public async Task LogEventAsync(
        AuditEventType eventType,
        string status,
        string targetUserName,
        string oracleInstanceName,
        bool isAuthorized,
        bool isSuccessful,
        string? detail = null,
        CancellationToken cancellationToken = default)
    {
        var audit = new FusionPasswordResetAudit
        {
            RequesterEmail = _userContextService.GetUserEmail(),
            OracleInstanceName = oracleInstanceName,
            TargetUserName = targetUserName,
            EventType = eventType,
            Status = status,
            Detail = detail,
            IsAuthorized = isAuthorized,
            IsSuccessful = isSuccessful,
            CreatedBy = _userContextService.GetUserEmail()
        };

        await _auditRepository.AddAsync(audit, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Audit event persisted for {EventType} user {TargetUserName} status {Status}",
            eventType,
            targetUserName,
            status);
    }
}
