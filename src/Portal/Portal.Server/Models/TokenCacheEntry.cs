namespace Dyvenix.App1.Portal.Server.Models;

public class TokenCacheEntry
{
	public required string AccessToken { get; set; }
	public required string RefreshToken { get; set; }
	public string? IdToken { get; set; }
	public DateTimeOffset ExpiresAt { get; set; }

	public bool IsExpiredOrNearExpiry(TimeSpan buffer) => DateTimeOffset.UtcNow >= ExpiresAt - buffer;
}
