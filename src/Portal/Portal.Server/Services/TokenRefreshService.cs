using Dyvenix.App1.Portal.Server.Models;
using System.Text.Json;

namespace Dyvenix.App1.Portal.Server.Services;

public class TokenRefreshService(
	IHttpClientFactory httpClientFactory,
	IConfiguration configuration,
	ILogger<TokenRefreshService> logger) : ITokenRefreshService
{
	public async Task<TokenCacheEntry?> RefreshTokenAsync(string refreshToken)
	{
		try
		{
			var authority = configuration["OpenIddict:Authority"]
				?? throw new InvalidOperationException("OpenIddict:Authority is not configured.");
			var clientId = configuration["OpenIddict:ClientId"]
				?? throw new InvalidOperationException("OpenIddict:ClientId is not configured.");
			var clientSecret = configuration["OpenIddict:ClientSecret"]
				?? throw new InvalidOperationException("OpenIddict:ClientSecret is not configured.");

			var tokenEndpoint = $"{authority.TrimEnd('/')}/connect/token";

			using var client = httpClientFactory.CreateClient();
			var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint)
			{
				Content = new FormUrlEncodedContent(new Dictionary<string, string>
				{
					["grant_type"] = "refresh_token",
					["refresh_token"] = refreshToken,
					["client_id"] = clientId,
					["client_secret"] = clientSecret
				})
			};

			var response = await client.SendAsync(request);

			if (!response.IsSuccessStatusCode)
			{
				var body = await response.Content.ReadAsStringAsync();
				logger.LogWarning("Token refresh failed with status {StatusCode}: {Body}",
					response.StatusCode, body);
				return null;
			}

			var json = await response.Content.ReadAsStringAsync();
			using var doc = JsonDocument.Parse(json);
			var root = doc.RootElement;

			var accessToken = root.GetProperty("access_token").GetString()!;
			var expiresIn = root.GetProperty("expires_in").GetInt32();
			var newRefreshToken = root.TryGetProperty("refresh_token", out var rt)
				? rt.GetString() ?? refreshToken
				: refreshToken;
			var idToken = root.TryGetProperty("id_token", out var idt)
				? idt.GetString()
				: null;

			return new TokenCacheEntry
			{
				AccessToken = accessToken,
				RefreshToken = newRefreshToken,
				IdToken = idToken,
				ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(expiresIn)
			};
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Exception during token refresh");
			return null;
		}
	}
}
