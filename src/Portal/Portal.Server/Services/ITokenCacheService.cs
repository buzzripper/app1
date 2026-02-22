using Dyvenix.App1.Portal.Server.Models;

namespace Dyvenix.App1.Portal.Server.Services;

public interface ITokenCacheService
{
	Task StoreTokensAsync(string sessionId, TokenCacheEntry entry);

	/// <summary>
	/// Returns a valid access token for the session, silently refreshing if near expiry.
	/// Returns null if no tokens are cached or refresh fails.
	/// </summary>
	Task<string?> GetAccessTokenAsync(string sessionId);

	Task<string?> GetIdTokenAsync(string sessionId);

	Task RemoveTokensAsync(string sessionId);
}
