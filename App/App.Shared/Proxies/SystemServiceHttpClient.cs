using Dyvenix.App.Shared.DTOs;
using Dyvenix.App.Shared.Interfaces;
using System.Net.Http.Json;

namespace Dyvenix.App.Shared.Proxies;

public class SystemServiceHttpClient : IAppSystemService
{
	public const string cUrlPathRoot = $"api/app/v1/system";

	private readonly HttpClient _httpClient;

	public SystemServiceHttpClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<string> Ping()
	{
		var response = await _httpClient.GetAsync($"{cUrlPathRoot}/ping");
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadAsStringAsync();
	}

	public async Task<AppHealthStatus> Health()
	{
		var response = await _httpClient.GetAsync($"{cUrlPathRoot}/health");
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadFromJsonAsync<AppHealthStatus>()
			?? throw new InvalidOperationException("Failed to deserialize health status");
	}
}
