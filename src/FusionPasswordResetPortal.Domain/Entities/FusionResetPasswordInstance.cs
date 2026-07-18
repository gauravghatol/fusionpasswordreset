using FusionPasswordResetPortal.Domain.Common;

namespace FusionPasswordResetPortal.Domain.Entities;

public class FusionResetPasswordInstance : BaseEntity
{
    public string InstanceCode { get; set; } = string.Empty;
    public string InstanceName { get; set; } = string.Empty;
    public string EnvironmentName { get; set; } = string.Empty;
    public string ApiBaseUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
}
