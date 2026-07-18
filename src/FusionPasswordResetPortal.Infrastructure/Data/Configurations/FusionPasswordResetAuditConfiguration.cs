using FusionPasswordResetPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FusionPasswordResetPortal.Infrastructure.Data.Configurations;

public class FusionPasswordResetAuditConfiguration : IEntityTypeConfiguration<FusionPasswordResetAudit>
{
    public void Configure(EntityTypeBuilder<FusionPasswordResetAudit> builder)
    {
        builder.ToTable("TblFusionUserPWDRest");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.RequesterEmail).HasMaxLength(256).IsRequired();
        builder.Property(x => x.OracleInstanceName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.TargetUserName).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Detail).HasMaxLength(4000);
        builder.Property(x => x.CreatedBy).HasMaxLength(256).IsRequired();
        builder.Property(x => x.ModifiedBy).HasMaxLength(256);
    }
}
