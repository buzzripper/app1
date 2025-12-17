using Microsoft.Identity.Web;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace App1.Portal.Server;

/// <summary>
/// YARP transform that forwards JWT access tokens from Portal's Microsoft Entra ID authentication
/// to downstream Auth/App services as Bearer tokens.
/// 
/// This enables a clean separation where:
/// - Portal handles cookie-based authentication for browser clients (Angular)
/// - Auth/App APIs only handle JWT Bearer authentication
/// - YARP acts as the bridge, converting cookies ? JWT tokens
/// 
/// Architecture Flow:
/// ???????????  Cookie   ??????????  JWT Transform  ????????????????
/// ? Angular ? ????????? ? Portal ? ???????????????? ? Auth/App API ?
/// ???????????           ??????????                  ????????????????
///                           ?                              ?
///                           ? Cookie Auth                  ? JWT Bearer
///                           ?                              ?
///                    ????????????????                      ?
///                    ? YARP Proxy   ? ??????????????????????
///                    ? + Transform  ?   (ITokenAcquisition)
///                    ????????????????
/// 
/// For In-Process Modules:
/// - YARP routes to http://localhost (same process)
/// - Transform still applies, adding JWT Bearer token
/// - Request reaches in-process controller with JWT auth
/// 
/// For Out-of-Process Modules:
/// - YARP routes to remote service URL
/// - Transform applies, adding JWT Bearer token
/// - Request reaches remote service with JWT auth
/// 
/// Direct API Access (Postman, Mobile Apps):
/// ????????????  JWT Bearer  ????????????????
/// ? Postman  ? ????????????? ? Auth/App API ?
/// ????????????               ????????????????
///                                   ? Validates JWT
///                                   ?
///                            Returns response
/// </summary>
public static class JwtForwardingTransformExtensions
{
	/// <summary>
	/// Adds a transform that automatically forwards access tokens to all proxied requests.
	/// The token is obtained from Portal's Microsoft Entra ID authentication via ITokenAcquisition.
	/// 
	/// This transform is applied to ALL requests going through YARP, including:
	/// - In-process Auth/App API calls (localhost routing)
	/// - Out-of-process Auth/App API calls (remote service routing)
	/// 
	/// The transform will:
	/// 1. Check if the user is authenticated (via Portal's cookie)
	/// 2. Use ITokenAcquisition to get a JWT access token for the user
	/// 3. Add the token as Authorization: Bearer {token} header
	/// 4. Forward the request to the downstream service
	/// </summary>
	/// <param name="builder">The reverse proxy builder.</param>
	/// <param name="downstreamScopes">The scopes to request when acquiring the access token.</param>
	/// <returns>The builder for chaining.</returns>
	public static IReverseProxyBuilder AddJwtForwardingTransform(
		this IReverseProxyBuilder builder,
		string[] downstreamScopes)
	{
		builder.AddTransforms(builderContext =>
		{
			builderContext.AddRequestTransform(async transformContext =>
			{
				// Only add token for authenticated users
				if (transformContext.HttpContext.User?.Identity?.IsAuthenticated != true)
				{
					return;
				}

				// Skip if there are no scopes configured
				if (downstreamScopes.Length == 0)
				{
					var logger = transformContext.HttpContext.RequestServices
						.GetRequiredService<ILogger<Program>>();
					logger.LogWarning("No downstream scopes configured for JWT forwarding. Requests will be forwarded without Bearer token.");
					return;
				}

				try
				{
					var tokenAcquisition = transformContext.HttpContext.RequestServices
						.GetRequiredService<ITokenAcquisition>();

					// Acquire access token for the downstream API
					// This uses the user's authenticated session (cookie) to get a JWT token
					var accessToken = await tokenAcquisition.GetAccessTokenForUserAsync(
						scopes: downstreamScopes,
						user: transformContext.HttpContext.User);

					// Add the token as a Bearer token to the proxied request
					if (!string.IsNullOrEmpty(accessToken))
					{
						transformContext.ProxyRequest.Headers.Authorization =
							new global::System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
					}
				}
				catch (MicrosoftIdentityWebChallengeUserException ex)
				{
					// Token cache miss - user needs to re-authenticate
					var logger = transformContext.HttpContext.RequestServices
						.GetRequiredService<ILogger<Program>>();
					logger.LogWarning(ex,
						"Token acquisition failed for proxied request to {Path}. User may need to re-authenticate.",
						transformContext.HttpContext.Request.Path);

					// Don't fail the request - let the downstream service return 401
				}
				catch (Exception ex)
				{
					var logger = transformContext.HttpContext.RequestServices
						.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex,
						"Unexpected error acquiring access token for proxied request to {Path}",
						transformContext.HttpContext.Request.Path);
				}
			});
		});

		return builder;
	}
}
