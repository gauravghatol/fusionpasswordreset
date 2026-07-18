using FusionPasswordResetPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FusionPasswordResetPortal.Infrastructure.Data.Configurations;

public class FusionResetPasswordInstanceConfiguration : IEntityTypeConfiguration<FusionResetPasswordInstance>
{
    public void Configure(EntityTypeBuilder<FusionResetPasswordInstance> builder)
    {
        builder.ToTable("TblFusionResetPWDInstance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.InstanceCode).HasMaxLength(50).IsRequired();
        builder.Property(x => x.InstanceName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.EnvironmentName).HasMaxLength(50).IsRequired();
        builder.Property(x => x.ApiBaseUrl).HasMaxLength(500).IsRequired();
        builder.Property(x => x.CreatedBy).HasMaxLength(256).IsRequired();
        builder.Property(x => x.ModifiedBy).HasMaxLength(256);
    }
}
