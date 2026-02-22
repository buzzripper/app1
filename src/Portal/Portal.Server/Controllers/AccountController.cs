using Dyvenix.App1.Portal.Server.Services;

namespace App1.App1.Portal.Server.Controllers;

[Route("api/[controller]")]
public class AccountController : ControllerBase
{
	private readonly ILogger<AccountController> _logger;
	private readonly IConfiguration _configuration;
	private readonly ITokenCacheService _tokenCacheService;

	public AccountController(
		ILogger<AccountController> logger,
		IConfiguration configuration,
		ITokenCacheService tokenCacheService)
	{
		_logger = logger;
		_configuration = configuration;
		_tokenCacheService = tokenCacheService;
	}

	[HttpGet("Login")]
	public ActionResult Login(string? returnUrl)
	{
		var properties = GetAuthProperties(returnUrl);
		return Challenge(properties);
	}

	[Authorize]
	[HttpGet("Logout")]
	public async Task<IActionResult> Logout(string? returnUrl)
	{
		var redirectUri = GetSafeRedirectUri(returnUrl, "/");
		var properties = new AuthenticationProperties { RedirectUri = redirectUri };

		// Retrieve id_token from server-side cache for id_token_hint
		var sessionId = User.FindFirst("token_session_id")?.Value;
		if (!string.IsNullOrEmpty(sessionId))
		{
			var idToken = await _tokenCacheService.GetIdTokenAsync(sessionId);
			if (!string.IsNullOrEmpty(idToken))
			{
				properties.Parameters["id_token_hint"] = idToken;
				_logger.LogInformation("Logout with id_token_hint");
			}

			// Clear cached tokens
			await _tokenCacheService.RemoveTokensAsync(sessionId);
		}

		return SignOut(
			properties,
			CookieAuthenticationDefaults.AuthenticationScheme,
			OpenIdConnectDefaults.AuthenticationScheme);
	}

	private AuthenticationProperties GetAuthProperties(string? returnUrl)
	{
		var redirectUri = GetSafeRedirectUri(returnUrl, "/");
		return new AuthenticationProperties { RedirectUri = redirectUri };
	}

	/// <summary>
	/// Validates and returns a safe redirect URI.
	/// Allows redirects to configured allowed origins (CORS origins) or relative paths.
	/// </summary>
	private string GetSafeRedirectUri(string? returnUrl, string defaultPath)
	{
		if (string.IsNullOrEmpty(returnUrl))
		{
			return defaultPath;
		}

		// Allow relative URLs
		if (Uri.IsWellFormedUriString(returnUrl, UriKind.Relative))
		{
			return returnUrl.StartsWith('/') ? returnUrl : $"/{returnUrl}";
		}

		// For absolute URLs, validate against allowed origins
		if (Uri.TryCreate(returnUrl, UriKind.Absolute, out var uri))
		{
			var allowedOrigins = _configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
			var origin = $"{uri.Scheme}://{uri.Authority}";

			if (allowedOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase))
			{
				return returnUrl;
			}

			_logger.LogWarning("Blocked redirect to non-allowed origin: {Origin}", origin);
		}

		return defaultPath;
	}
}
