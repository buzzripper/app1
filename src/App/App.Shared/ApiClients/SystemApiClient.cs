using Dyvenix.App1.App.Shared.Contracts;
using Dyvenix.App1.App.Shared.DTOs;
using System.Net.Http.Json;

namespace Dyvenix.App1.App.Shared.ApiClients;

public class SystemApiClient : IAppSystemService
{
	public const string cUrlPathRoot = $"api/app/system";

	private readonly HttpClient _httpClient;

	public SystemApiClient(HttpClient httpClient)
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
