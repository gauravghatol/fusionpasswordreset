namespace FusionPasswordResetPortal.Application.DTOs;

public class OracleAccessTokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public int ExpiresInSeconds { get; set; }
}
