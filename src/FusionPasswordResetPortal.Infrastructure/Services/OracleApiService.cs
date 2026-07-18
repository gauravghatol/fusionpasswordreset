using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FusionPasswordResetPortal.Application.Common.Models;
using FusionPasswordResetPortal.Application.DTOs;
using FusionPasswordResetPortal.Application.Interfaces;
using FusionPasswordResetPortal.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FusionPasswordResetPortal.Infrastructure.Services;

public class OracleApiService : IOracleApiService
{
    private readonly HttpClient _httpClient;
    private readonly IFusionInstanceRepository _instanceRepository;
    private readonly SecretProvider _secretProvider;
    private readonly OracleApiOptions _options;
    private readonly ILogger<OracleApiService> _logger;

    public OracleApiService(
        HttpClient httpClient,
        IFusionInstanceRepository instanceRepository,
        SecretProvider secretProvider,
        IOptions<OracleApiOptions> options,
        ILogger<OracleApiService> logger)
    {
        _httpClient = httpClient;
        _instanceRepository = instanceRepository;
        _secretProvider = secretProvider;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<OracleAccessTokenDto> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        var clientId = await _secretProvider.GetSecretAsync(_options.ClientIdSecretName, cancellationToken);
        var clientSecret = await _secretProvider.GetSecretAsync(_options.ClientSecretSecretName, cancellationToken);

        var form = new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["scope"] = _options.Scope
        };

        using var response = await _httpClient.PostAsync(_options.TokenEndpoint, new FormUrlEncodedContent(form), cancellationToken);
        response.EnsureSuccessStatusCode();

        var token = await response.Content.ReadFromJsonAsync<OracleAccessTokenDto>(cancellationToken: cancellationToken)
                    ?? throw new InvalidOperationException("Oracle token response was empty.");

        return token;
    }

    public async Task<OperationResult<FusionUserSearchResultDto>> SearchUserAsync(int instanceId, string userNameOrEmail, CancellationToken cancellationToken = default)
    {
        try
        {
            var token = await GetAccessTokenAsync(cancellationToken);
            var instance = await _instanceRepository.GetByIdAsync(instanceId, cancellationToken)
                           ?? throw new InvalidOperationException($"Fusion instance {instanceId} was not found.");
            var subscriptionKey = await _secretProvider.GetSecretAsync(_options.SubscriptionKeySecretName, cancellationToken);

            using var request = new HttpRequestMessage(HttpMethod.Get, $"{instance.ApiBaseUrl.TrimEnd('/')}/{_options.SearchUserEndpoint.TrimStart('/')}?userNameOrEmail={Uri.EscapeDataString(userNameOrEmail)}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var payload = await response.Content.ReadFromJsonAsync<FusionUserSearchResultDto>(cancellationToken: cancellationToken);
            return payload is null
                ? OperationResult<FusionUserSearchResultDto>.Failure("Oracle user search returned no payload.")
                : OperationResult<FusionUserSearchResultDto>.Success(payload, "Oracle user located successfully.");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Oracle user search failed for {UserNameOrEmail}", userNameOrEmail);
            return OperationResult<FusionUserSearchResultDto>.Failure("Oracle user search failed. Please try again or contact support.");
        }
    }

    public async Task<OperationResult> ResetPasswordAsync(ResetPasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            var token = await GetAccessTokenAsync(cancellationToken);
            var instance = await _instanceRepository.GetByIdAsync(request.InstanceId, cancellationToken)
                           ?? throw new InvalidOperationException($"Fusion instance {request.InstanceId} was not found.");
            var subscriptionKey = await _secretProvider.GetSecretAsync(_options.SubscriptionKeySecretName, cancellationToken);

            using var message = new HttpRequestMessage(HttpMethod.Post, $"{instance.ApiBaseUrl.TrimEnd('/')}/{_options.ResetPasswordEndpoint.TrimStart('/')}")
            {
                Content = JsonContent.Create(new
                {
                    request.UserName,
                    request.Email,
                    NewPassword = request.NewPassword
                })
            };

            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            message.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            using var response = await _httpClient.SendAsync(message, cancellationToken);
            response.EnsureSuccessStatusCode();

            return OperationResult.Success("Oracle Fusion password reset completed successfully.");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Oracle password reset failed for {UserName}", request.UserName);
            return OperationResult.Failure("Oracle Fusion password reset failed. Please try again or contact support.");
        }
    }
}
