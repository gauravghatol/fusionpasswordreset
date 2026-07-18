using FusionPasswordResetPortal.Application.DTOs;

namespace FusionPasswordResetPortal.Web.Models.ViewModels;

public class DashboardViewModel
{
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IReadOnlyList<FusionInstanceDto> Instances { get; set; } = Array.Empty<FusionInstanceDto>();
}
