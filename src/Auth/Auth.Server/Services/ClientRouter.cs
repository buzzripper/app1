using Dyvenix.App1.App.Shared.Contracts.v1;

namespace Dyvenix.App1.Auth.Server.Services
{
	public interface IClientRouter
	{
		Task<HttpClient> GetHttpClient(string clientKey);
	}

	public class ClientRouter : IClientRouter
	{
		private readonly IClientService _clientService;
		private readonly Dictionary<string, string> _cache;
		private DateTime _expirationTimeUtc;

		public ClientRouter(IClientService clientService)
		{
			_cache = new Dictionary<string, string>();
			_expirationTimeUtc = DateTime.UtcNow.AddDays(-1);
			_clientService = clientService;
		}

		public async Task<HttpClient> GetHttpClient(string clientKey)
		{
			if (_expirationTimeUtc.CompareTo(DateTime.UtcNow) < 0)
				await this.RefreshCache();

			if (!_cache.TryGetValue(clientKey, out var baseUrl))
				throw new Exception($"Client with key '{clientKey}' not found.");

			return new HttpClient { BaseAddress = new Uri(baseUrl) };
		}

		private async Task RefreshCache()
		{
			_cache.Clear();

			var clientRouteDtos = await _clientService.GetAllRoutes();
			foreach (var clientRouteDto in clientRouteDtos)
				_cache.Add(clientRouteDto.Key, clientRouteDto.BaseUrl);

			_expirationTimeUtc = DateTime.UtcNow.AddMinutes(5);
		}
	}
}
