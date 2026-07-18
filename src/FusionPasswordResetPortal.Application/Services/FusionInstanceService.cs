using FusionPasswordResetPortal.Application.DTOs;
using FusionPasswordResetPortal.Application.Interfaces;

namespace FusionPasswordResetPortal.Application.Services;

public class FusionInstanceService : IFusionInstanceService
{
    private readonly IFusionInstanceRepository _repository;

    public FusionInstanceService(IFusionInstanceRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<FusionInstanceDto>> GetActiveInstancesAsync(CancellationToken cancellationToken = default)
    {
        var instances = await _repository.GetActiveInstancesAsync(cancellationToken);
        return instances
            .Select(instance => new FusionInstanceDto
            {
                Id = instance.Id,
                InstanceCode = instance.InstanceCode,
                InstanceName = instance.InstanceName,
                EnvironmentName = instance.EnvironmentName
            })
            .ToList();
    }
}
