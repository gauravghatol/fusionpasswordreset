using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FusionPasswordResetPortal.Infrastructure.Services;

public class SecretProvider
{
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _cache;
    private readonly ILogger<SecretProvider> _logger;

    public SecretProvider(IConfiguration configuration, IMemoryCache cache, ILogger<SecretProvider> logger)
    {
        _configuration = configuration;
        _cache = cache;
        _logger = logger;
    }

    public async Task<string> GetSecretAsync(string secretName, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(secretName, out string? cachedSecret) && !string.IsNullOrWhiteSpace(cachedSecret))
        {
            return cachedSecret;
        }

        var vaultUri = _configuration["KeyVault:VaultUri"] ?? throw new InvalidOperationException("Key Vault URI is not configured.");
        var client = new SecretClient(new Uri(vaultUri), new DefaultAzureCredential());
        var secret = await client.GetSecretAsync(secretName, cancellationToken: cancellationToken);

        _cache.Set(secretName, secret.Value.Value, TimeSpan.FromMinutes(15));
        _logger.LogInformation("Secret {SecretName} resolved from Key Vault", secretName);

        return secret.Value.Value;
    }
}
