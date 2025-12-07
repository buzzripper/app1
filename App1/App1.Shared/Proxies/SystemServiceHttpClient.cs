using Dyvenix.App1.Shared.DTOs;
using Dyvenix.App1.Shared.Interfaces;
using System.Net.Http.Json;

namespace Dyvenix.App1.Shared.Proxies;

public class SystemServiceHttpClient : ISystemService
{
	public const string cUrlPathRoot = "api/app1/v1/system";

	private readonly HttpClient _httpClient;

	public SystemServiceHttpClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<string> Alive()
	{
		var response = await _httpClient.GetAsync($"{cUrlPathRoot}/alive");
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadAsStringAsync();
	}

	public async Task<App1HealthStatus> Health()
	{
		var response = await _httpClient.GetAsync($"{cUrlPathRoot}/health");
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadFromJsonAsync<App1HealthStatus>()
			?? throw new InvalidOperationException("Failed to deserialize health status");
	}
}
