using Dyvenix.App1.Common.Shared.Contracts;
using Dyvenix.App1.Common.Shared.DTOs;
using System.Net.Http.Json;

namespace Dyvenix.App1.App.Shared.ApiClients;

public class AppSystemApiClient : ISystemService
{
    public const string cUrlPathRoot = $"api/app/system";

    private readonly HttpClient _httpClient;

    public AppSystemApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> Ping()
    {
        var response = await _httpClient.GetAsync($"{cUrlPathRoot}/ping");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<HealthStatus> Health()
    {
        var response = await _httpClient.GetAsync($"{cUrlPathRoot}/health");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<HealthStatus>()
            ?? throw new InvalidOperationException("Failed to deserialize health status");
    }

    public async Task<ServiceInfo> GetServiceInfo()
    {
        var response = await _httpClient.GetAsync($"{cUrlPathRoot}/getserviceinfo");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ServiceInfo>()
            ?? throw new InvalidOperationException("Failed to deserialize health status");
    }
}
