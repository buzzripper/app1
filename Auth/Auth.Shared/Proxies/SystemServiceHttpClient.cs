using Dyvenix.Auth.Shared.DTOs;
using Dyvenix.Auth.Shared.Interfaces;
using System.Net.Http.Json;

namespace Dyvenix.Auth.Shared.Proxies;

public class SystemServiceHttpClient : IAuthSystemService
{
	public const string cUrlPathRoot = "api/auth/v1/system";

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

	public async Task<AuthHealthStatus> Health()
	{
		var response = await _httpClient.GetAsync($"{cUrlPathRoot}/health");
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadFromJsonAsync<AuthHealthStatus>()
			?? throw new InvalidOperationException("Failed to deserialize health status");
	}
}
