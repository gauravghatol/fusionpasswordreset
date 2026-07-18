using FusionPasswordResetPortal.Application.DTOs;

namespace FusionPasswordResetPortal.Application.Interfaces;

public interface IFusionInstanceService
{
    Task<IReadOnlyList<FusionInstanceDto>> GetActiveInstancesAsync(CancellationToken cancellationToken = default);
}
