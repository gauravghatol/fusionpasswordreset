using FusionPasswordResetPortal.Application.Common.Models;
using FusionPasswordResetPortal.Application.DTOs;

namespace FusionPasswordResetPortal.Application.Interfaces;

public interface IOracleApiService
{
    Task<OracleAccessTokenDto> GetAccessTokenAsync(CancellationToken cancellationToken = default);
    Task<OperationResult<FusionUserSearchResultDto>> SearchUserAsync(int instanceId, string userNameOrEmail, CancellationToken cancellationToken = default);
    Task<OperationResult> ResetPasswordAsync(ResetPasswordRequestDto request, CancellationToken cancellationToken = default);
}
