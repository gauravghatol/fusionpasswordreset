using FusionPasswordResetPortal.Application.Common.Models;
using FusionPasswordResetPortal.Application.DTOs;

namespace FusionPasswordResetPortal.Application.Interfaces;

public interface IPasswordResetOrchestrator
{
    Task<OperationResult<FusionUserSearchResultDto>> SearchUserAsync(int instanceId, string userNameOrEmail, CancellationToken cancellationToken = default);
    Task<OperationResult> ResetPasswordAsync(ResetPasswordRequestDto request, CancellationToken cancellationToken = default);
}
