namespace Dyvenix.App1.Portal.Server.Controllers;

[Route("api/[controller]")]
public class AccountController : ControllerBase
{
	private readonly ILogger<AccountController> _logger;
	private readonly IConfiguration _configuration;

	public AccountController(ILogger<AccountController> logger, IConfiguration configuration)
	{
		_logger = logger;
		_configuration = configuration;
	}

	[HttpGet("Login")]
	public ActionResult Login(string? returnUrl, string? claimsChallenge)
	{
		var properties = GetAuthProperties(returnUrl);

		if (claimsChallenge != null)
		{
			string jsonString = claimsChallenge.Replace("\\", "").Trim('"');
			properties.Items["claims"] = jsonString;
		}

		return Challenge(properties);
	}

	[Authorize]
	[HttpGet("Logout")]
	public async Task<IActionResult> Logout(string? returnUrl)
	{
		var redirectUri = GetSafeRedirectUri(returnUrl, "/");

		var properties = new AuthenticationProperties { RedirectUri = redirectUri };

		// Get id_token from authentication properties to skip account selection prompt
		var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		var idToken = authenticateResult?.Properties?.GetTokenValue("id_token");

		if (!string.IsNullOrEmpty(idToken))
		{
			properties.Parameters["id_token_hint"] = idToken;
			_logger.LogInformation("Logout with id_token_hint");

			// Also add logout_hint as a fallback/additional hint
			var email = User.FindFirst("preferred_username")?.Value
					 ?? User.FindFirst("email")?.Value
					 ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

			if (!string.IsNullOrEmpty(email))
			{
				properties.Parameters["logout_hint"] = email;
				_logger.LogInformation("Also adding logout_hint: {Email}", email);
			}
		}
		else
		{
			_logger.LogWarning("id_token not found in authentication properties");
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
