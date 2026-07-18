using FusionPasswordResetPortal.Application.Interfaces;
using FusionPasswordResetPortal.Domain.Entities;
using FusionPasswordResetPortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FusionPasswordResetPortal.Infrastructure.Repositories;

public class FusionInstanceRepository : Repository<FusionResetPasswordInstance>, IFusionInstanceRepository
{
    public FusionInstanceRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<IReadOnlyList<FusionResetPasswordInstance>> GetActiveInstancesAsync(CancellationToken cancellationToken = default)
    {
        return await Context.FusionResetPasswordInstances
            .AsNoTracking()
            .Where(instance => instance.IsActive)
            .OrderBy(instance => instance.SortOrder)
            .ToListAsync(cancellationToken);
    }
}
