using System.Net.Http.Json;
using App1.Shared.DTOs;
using App1.Shared.Interfaces;

namespace App1.Shared.Proxies;

public class SystemServiceHttpClient : ISystemService
{
    private readonly HttpClient _httpClient;

    public SystemServiceHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HealthStatus> CheckHealth()
    {
        var response = await _httpClient.GetAsync("/api/app1/health");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<HealthStatus>() 
            ?? throw new InvalidOperationException("Failed to deserialize health status");
    }
}
