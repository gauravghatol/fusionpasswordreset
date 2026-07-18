namespace FusionPasswordResetPortal.Application.DTOs;

public class ResetPasswordRequestDto
{
    public int InstanceId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}
