using FluentValidation;
using FusionPasswordResetPortal.Application.Common.Models;
using FusionPasswordResetPortal.Application.DTOs;
using FusionPasswordResetPortal.Application.Interfaces;
using FusionPasswordResetPortal.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace FusionPasswordResetPortal.Application.Services;

public class PasswordResetOrchestrator : IPasswordResetOrchestrator
{
    private readonly IOracleApiService _oracleApiService;
    private readonly IUserAuthorizationService _userAuthorizationService;
    private readonly IUserContextService _userContextService;
    private readonly IValidationService _validationService;
    private readonly IAuditService _auditService;
    private readonly IValidator<ResetPasswordRequestDto> _validator;
    private readonly ILogger<PasswordResetOrchestrator> _logger;

    public PasswordResetOrchestrator(
        IOracleApiService oracleApiService,
        IUserAuthorizationService userAuthorizationService,
        IUserContextService userContextService,
        IValidationService validationService,
        IAuditService auditService,
        IValidator<ResetPasswordRequestDto> validator,
        ILogger<PasswordResetOrchestrator> logger)
    {
        _oracleApiService = oracleApiService;
        _userAuthorizationService = userAuthorizationService;
        _userContextService = userContextService;
        _validationService = validationService;
        _auditService = auditService;
        _validator = validator;
        _logger = logger;
    }

    public async Task<OperationResult<FusionUserSearchResultDto>> SearchUserAsync(int instanceId, string userNameOrEmail, CancellationToken cancellationToken = default)
    {
        var requester = _userContextService.GetUserEmail();
        var isAuthorized = await _userAuthorizationService.IsAuthorizedAsync(requester, cancellationToken);
        if (!isAuthorized)
        {
            await _auditService.LogEventAsync(AuditEventType.SecurityEvent, "Forbidden", userNameOrEmail, $"Instance-{instanceId}", false, false, "Requester not authorized.", cancellationToken);
            return OperationResult<FusionUserSearchResultDto>.Failure("You are not authorized to search Oracle users.");
        }

        if (_validationService.IsRestrictedAccount(userNameOrEmail))
        {
            await _auditService.LogEventAsync(AuditEventType.SecurityEvent, "Rejected", userNameOrEmail, $"Instance-{instanceId}", true, false, "Restricted account search blocked.", cancellationToken);
            return OperationResult<FusionUserSearchResultDto>.Failure("Restricted account operations are not allowed.");
        }

        var result = await _oracleApiService.SearchUserAsync(instanceId, userNameOrEmail, cancellationToken);
        await _auditService.LogEventAsync(AuditEventType.UserSearch, result.Succeeded ? "Completed" : "Failed", userNameOrEmail, $"Instance-{instanceId}", true, result.Succeeded, result.Message, cancellationToken);

        return result;
    }

    public async Task<OperationResult> ResetPasswordAsync(ResetPasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return OperationResult.Failure(string.Join("; ", validation.Errors.Select(error => error.ErrorMessage)));
        }

        var requester = _userContextService.GetUserEmail();
        var isAuthorized = await _userAuthorizationService.IsAuthorizedAsync(requester, cancellationToken);
        if (!isAuthorized)
        {
            await _auditService.LogEventAsync(AuditEventType.SecurityEvent, "Forbidden", request.UserName, $"Instance-{request.InstanceId}", false, false, "Requester not authorized.", cancellationToken);
            return OperationResult.Failure("You are not authorized to reset passwords.");
        }

        if (_validationService.IsRestrictedAccount(request.UserName) || _validationService.IsRestrictedAccount(request.Email))
        {
            return OperationResult.Failure("Restricted account password resets are not allowed.");
        }

        var result = await _oracleApiService.ResetPasswordAsync(request, cancellationToken);
        await _auditService.LogEventAsync(AuditEventType.PasswordReset, result.Succeeded ? "Completed" : "Failed", request.UserName, $"Instance-{request.InstanceId}", true, result.Succeeded, result.Message, cancellationToken);

        _logger.LogInformation("Password reset orchestration completed for {UserName} with success {Succeeded}", request.UserName, result.Succeeded);
        return result;
    }
}
