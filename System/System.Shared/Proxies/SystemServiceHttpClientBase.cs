//using Dyvenix.System.Shared.DTOs;
//using Dyvenix.System.Shared.Interfaces;
//using System.Net.Http.Json;

//namespace Dyvenix.System.Shared.Proxies;

//public abstract class SystemServiceHttpClientBase : ISystemService
//{
//	private readonly HttpClient _httpClient;
//	private readonly string _urlPathRoot;

//	public SystemServiceHttpClientBase(HttpClient httpClient, string apiName)
//	{
//		_httpClient = httpClient;
//		_urlPathRoot = $"/{apiName?.ToLower()}";
//	}

//	public async Task<bool> Alive()
//	{
//		var response = await _httpClient.GetAsync($"{_urlPathRoot}/alive");
//		response.EnsureSuccessStatusCode();
//		return true;
//	}

//	public async Task<HealthStatus> Health()
//	{
//		var response = await _httpClient.GetAsync($"{_urlPathRoot}/health");
//		response.EnsureSuccessStatusCode();
//		return await response.Content.ReadFromJsonAsync<HealthStatus>()
//			?? throw new InvalidOperationException("Failed to deserialize health status");
//	}

//	Task<HealthStatus> ISystemService.Health()
//	{
//		throw new NotImplementedException();
//	}
//}
