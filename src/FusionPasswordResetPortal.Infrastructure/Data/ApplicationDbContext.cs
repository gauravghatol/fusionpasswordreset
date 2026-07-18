using FusionPasswordResetPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FusionPasswordResetPortal.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<FusionPasswordResetAudit> FusionPasswordResetAudits => Set<FusionPasswordResetAudit>();
    public DbSet<FusionResetPasswordInstance> FusionResetPasswordInstances => Set<FusionResetPasswordInstance>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
