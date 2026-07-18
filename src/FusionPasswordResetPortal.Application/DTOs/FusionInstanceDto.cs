namespace FusionPasswordResetPortal.Application.DTOs;

public class FusionInstanceDto
{
    public int Id { get; set; }
    public string InstanceCode { get; set; } = string.Empty;
    public string InstanceName { get; set; } = string.Empty;
    public string EnvironmentName { get; set; } = string.Empty;
}
