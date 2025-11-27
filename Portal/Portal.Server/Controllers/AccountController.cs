namespace BffMicrosoftEntraID.Server.Controllers;

// orig src https://github.com/berhir/BlazorWebAssemblyCookieAuth
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
	private readonly ILogger<AccountController> _logger;

	public AccountController(ILogger<AccountController> logger)
	{
		_logger = logger;
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
	public async Task<IActionResult> Logout()
	{
		var properties = new AuthenticationProperties { RedirectUri = "/sign-out" };
		
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

	/// <summary>
	/// Original src:
	/// https://github.com/dotnet/blazor-samples/blob/main/8.0/BlazorWebOidc/BlazorWebOidc/LoginLogoutEndpointRouteBuilderExtensions.cs
	/// </summary>
	private static AuthenticationProperties GetAuthProperties(string? returnUrl)
	{
		const string pathBase = "/";

		// Prevent open redirects.
		if (string.IsNullOrEmpty(returnUrl))
		{
			returnUrl = pathBase;
		}
		else if (!Uri.IsWellFormedUriString(returnUrl, UriKind.Relative))
		{
			returnUrl = new Uri(returnUrl, UriKind.Absolute).PathAndQuery;
		}
		else if (returnUrl[0] != '/')
		{
			returnUrl = $"{pathBase}{returnUrl}";
		}

		return new AuthenticationProperties { RedirectUri = returnUrl };
	}
}
