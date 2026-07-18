using FusionPasswordResetPortal.Domain.Entities;

namespace FusionPasswordResetPortal.Application.Interfaces;

public interface IFusionInstanceRepository : IRepository<FusionResetPasswordInstance>
{
    Task<IReadOnlyList<FusionResetPasswordInstance>> GetActiveInstancesAsync(CancellationToken cancellationToken = default);
}
