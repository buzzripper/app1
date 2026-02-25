using Dyvenix.App1.AdAgent.Shared.Contracts;
using Dyvenix.App1.Common.Shared.DTOs;
using System.Net.Http.Json;

namespace Dyvenix.App1.AdAgent.Shared.ApiClients;

public class AdAgentSystemApiClient : IAdAgentSystemService
{
    public const string cUrlPathRoot = $"api/adagent/system";

    private readonly HttpClient _httpClient;

    public AdAgentSystemApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HealthStatus> Health()
    {
        var response = await _httpClient.GetAsync($"{cUrlPathRoot}/health");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<HealthStatus>()
            ?? throw new InvalidOperationException("Failed to deserialize health status");
    }
}
