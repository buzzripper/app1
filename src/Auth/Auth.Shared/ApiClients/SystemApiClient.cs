using Dyvenix.App1.Auth.Shared.Contracts;
using Dyvenix.App1.Auth.Shared.DTOs;
using System.Net.Http.Json;

namespace Dyvenix.App1.Auth.Shared.ApiClients;

public class SystemApiClient : IAuthSystemService
{
	public const string cUrlPathRoot = $"api/auth/system";

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

	public async Task<AuthHealthStatus> Health()
	{
		var response = await _httpClient.GetAsync($"{cUrlPathRoot}/health");
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadFromJsonAsync<AuthHealthStatus>()
			?? throw new InvalidOperationException("Failed to deserialize health status");
	}
}
