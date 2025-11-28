using System.Net.Http.Json;
using Auth.Shared.DTOs;
using Auth.Shared.Interfaces;

namespace Auth.Shared.Proxies;

public class SystemServiceHttpClient : ISystemService
{
    private readonly HttpClient _httpClient;

    public SystemServiceHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HealthStatus> CheckHealth()
    {
        var response = await _httpClient.GetAsync("/api/auth/health");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<HealthStatus>() 
            ?? throw new InvalidOperationException("Failed to deserialize health status");
    }
}
