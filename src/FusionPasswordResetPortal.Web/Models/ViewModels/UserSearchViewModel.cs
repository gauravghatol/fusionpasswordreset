using System.ComponentModel.DataAnnotations;
using FusionPasswordResetPortal.Application.DTOs;

namespace FusionPasswordResetPortal.Web.Models.ViewModels;

public class UserSearchViewModel
{
    [Required]
    public int InstanceId { get; set; }

    [Required]
    [Display(Name = "Oracle Username or Email")]
    public string UserNameOrEmail { get; set; } = string.Empty;

    public IReadOnlyList<FusionInstanceDto> Instances { get; set; } = Array.Empty<FusionInstanceDto>();
    public FusionUserSearchResultDto? SearchResult { get; set; }
    public string? StatusMessage { get; set; }
}
