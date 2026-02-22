using Dyvenix.App1.Portal.Server.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Dyvenix.App1.Portal.Server.Services;

public class TokenCacheService(
	IDistributedCache cache,
	ITokenRefreshService tokenRefreshService,
	ILogger<TokenCacheService> logger) : ITokenCacheService
{
	private static readonly TimeSpan RefreshBuffer = TimeSpan.FromSeconds(60);

	private static readonly DistributedCacheEntryOptions CacheOptions = new()
	{
		// Sliding expiration: remove entry if not accessed for a while
		SlidingExpiration = TimeSpan.FromHours(1),
		// Absolute expiration: hard cap to prevent stale entries
		AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(8)
	};

	public async Task StoreTokensAsync(string sessionId, TokenCacheEntry entry)
	{
		var json = JsonSerializer.Serialize(entry);
		await cache.SetStringAsync(GetKey(sessionId), json, CacheOptions);
		logger.LogDebug("Stored tokens for session {SessionId}, expires at {ExpiresAt}", sessionId, entry.ExpiresAt);
	}

	public async Task<string?> GetAccessTokenAsync(string sessionId)
	{
		var entry = await GetEntryAsync(sessionId);
		if (entry is null)
		{
			logger.LogDebug("No cached tokens found for session {SessionId}", sessionId);
			return null;
		}

		if (!entry.IsExpiredOrNearExpiry(RefreshBuffer))
			return entry.AccessToken;

		// Token is expired or near expiry — attempt silent refresh
		logger.LogDebug("Access token near expiry for session {SessionId}, refreshing", sessionId);
		var refreshed = await tokenRefreshService.RefreshTokenAsync(entry.RefreshToken);

		if (refreshed is null)
		{
			logger.LogWarning("Token refresh failed for session {SessionId}", sessionId);
			await RemoveTokensAsync(sessionId);
			return null;
		}

		await StoreTokensAsync(sessionId, refreshed);
		return refreshed.AccessToken;
	}

	public async Task<string?> GetIdTokenAsync(string sessionId)
	{
		var entry = await GetEntryAsync(sessionId);
		return entry?.IdToken;
	}

	public async Task RemoveTokensAsync(string sessionId)
	{
		await cache.RemoveAsync(GetKey(sessionId));
		logger.LogDebug("Removed cached tokens for session {SessionId}", sessionId);
	}

	private async Task<TokenCacheEntry?> GetEntryAsync(string sessionId)
	{
		var json = await cache.GetStringAsync(GetKey(sessionId));
		return json is null ? null : JsonSerializer.Deserialize<TokenCacheEntry>(json);
	}

	private static string GetKey(string sessionId) => $"token:{sessionId}";
}
