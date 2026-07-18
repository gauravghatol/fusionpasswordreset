using FusionPasswordResetPortal.Application.Interfaces;
using FusionPasswordResetPortal.Domain.Entities;
using FusionPasswordResetPortal.Infrastructure.Data;

namespace FusionPasswordResetPortal.Infrastructure.Repositories;

public class AuditRepository : Repository<FusionPasswordResetAudit>, IAuditRepository
{
    public AuditRepository(ApplicationDbContext context)
        : base(context)
    {
    }
}
