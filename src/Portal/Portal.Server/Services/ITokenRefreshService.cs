using Dyvenix.App1.Portal.Server.Models;

namespace Dyvenix.App1.Portal.Server.Services;

public interface ITokenRefreshService
{
	Task<TokenCacheEntry?> RefreshTokenAsync(string refreshToken);
}
