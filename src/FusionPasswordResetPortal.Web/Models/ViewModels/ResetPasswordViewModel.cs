using System.ComponentModel.DataAnnotations;
using FusionPasswordResetPortal.Application.DTOs;

namespace FusionPasswordResetPortal.Web.Models.ViewModels;

public class ResetPasswordViewModel
{
    [Required]
    public int InstanceId { get; set; }

    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = string.Empty;

    public IReadOnlyList<FusionInstanceDto> Instances { get; set; } = Array.Empty<FusionInstanceDto>();

    public ResetPasswordRequestDto ToDto() =>
        new()
        {
            InstanceId = InstanceId,
            UserName = UserName,
            Email = Email,
            NewPassword = NewPassword,
            ConfirmPassword = ConfirmPassword
        };
}
