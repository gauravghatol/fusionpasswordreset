namespace FusionPasswordResetPortal.Infrastructure.Options;

public class OracleApiOptions
{
    public const string SectionName = "OracleApi";

    public string BaseUrl { get; set; } = string.Empty;
    public string TokenEndpoint { get; set; } = string.Empty;
    public string SearchUserEndpoint { get; set; } = string.Empty;
    public string ResetPasswordEndpoint { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public string SubscriptionKeySecretName { get; set; } = string.Empty;
    public string ClientIdSecretName { get; set; } = string.Empty;
    public string ClientSecretSecretName { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 30;
}
