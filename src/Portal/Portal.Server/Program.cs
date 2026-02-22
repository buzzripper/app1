#if AUTH_INPROCESS
#endif
#if APP_INPROCESS
using Dyvenix.App1.App.Api.Extensions;
#endif
using App1.App1.Portal.Server;
using App1.App1.Portal.Server.Interfaces;
using App1.App1.Portal.Server.Services;
using Dyvenix.App1.Common.Api.Extensions;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.Portal.Server.Logging;
using Dyvenix.App1.Portal.Server.Services;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http.Headers;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (OpenTelemetry, health checks, service discovery, resilience)
builder.AddServiceDefaults();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
	serverOptions.AddServerHeader = false;
});

builder.Services.AddOpenApi();

var services = builder.Services;
var configuration = builder.Configuration;

// Configure CORS for cross-origin Angular app
var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		policy.WithOrigins(allowedOrigins)
			.AllowAnyHeader()
			.AllowAnyMethod()
			.AllowCredentials();
	});
});

services.AddSecurityHeaderPolicies()
	.SetPolicySelector(ctx =>
	{
		return ApiSecurityHeadersDefinitions.GetHeaderPolicyCollection(builder.Environment.IsDevelopment());
	});

services.AddAntiforgery(options =>
{
	options.HeaderName = "X-XSRF-TOKEN";
	options.Cookie.Name = "__Host-X-XSRF-TOKEN";
	options.Cookie.SameSite = SameSiteMode.Lax;
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

services.AddScoped<IPortalSystemService, PortalSystemService>();
services.AddScoped<ApiExceptionFilter<PortalSystemService>>();

services.AddHttpClient();
services.AddOptions();

// Token cache infrastructure (backed by IDistributedCache — swap to Redis/SQL later via DI)
services.AddDistributedMemoryCache();
services.AddScoped<ITokenRefreshService, TokenRefreshService>();
services.AddScoped<ITokenCacheService, TokenCacheService>();

// Authentication: Cookie + OpenID Connect against OpeniddictServer
services.AddAuthentication(options =>
{
	options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
	options.Cookie.SameSite = SameSiteMode.Lax;
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
	options.Cookie.HttpOnly = true;
})
.AddOpenIdConnect(options =>
{
	options.Authority = configuration["OpenIddict:Authority"];
	options.ClientId = configuration["OpenIddict:ClientId"];
	options.ClientSecret = configuration["OpenIddict:ClientSecret"];
	options.ResponseType = OpenIdConnectResponseType.Code;
	options.UsePkce = true;
	options.SaveTokens = false;

	options.Scope.Clear();
	foreach (var scope in (configuration["OpenIddict:Scopes"] ?? "openid profile email roles app1-api")
		.Split(' ', StringSplitOptions.RemoveEmptyEntries))
	{
		options.Scope.Add(scope);
	}

	options.CallbackPath = configuration["OpenIddict:CallbackPath"] ?? "/signin-oidc";
	options.SignedOutCallbackPath = configuration["OpenIddict:SignedOutCallbackPath"] ?? "/signout-callback-oidc";

	options.MapInboundClaims = false;
	options.TokenValidationParameters.NameClaimType = "name";
	options.TokenValidationParameters.RoleClaimType = "role";

	options.Events = new OpenIdConnectEvents
	{
		OnTokenValidated = async context =>
		{
			var tokenResponse = context.TokenEndpointResponse;
			if (tokenResponse is null)
				return;

			var accessToken = tokenResponse.AccessToken;
			var refreshToken = tokenResponse.RefreshToken;
			var idToken = tokenResponse.IdToken;
			var expiresIn = tokenResponse.ExpiresIn;

			if (string.IsNullOrEmpty(accessToken))
				return;

			var expiry = string.IsNullOrEmpty(expiresIn)
				? DateTimeOffset.UtcNow.AddMinutes(60)
				: DateTimeOffset.UtcNow.AddSeconds(int.Parse(expiresIn));

			// Generate a unique session ID and store it as a claim
			var sessionId = Guid.NewGuid().ToString("N");
			var identity = (ClaimsIdentity?)context.Principal?.Identity;
			identity?.AddClaim(new Claim("token_session_id", sessionId));

			// Store tokens server-side
			var tokenCache = context.HttpContext.RequestServices.GetRequiredService<ITokenCacheService>();
			await tokenCache.StoreTokensAsync(sessionId, new Dyvenix.App1.Portal.Server.Models.TokenCacheEntry
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken ?? "",
				IdToken = idToken,
				ExpiresAt = expiry
			});
		}
	};
});

services.AddControllers();

services.AddTransient<IPortalModuleLogger>(sp => new PortalModuleLogger(sp.GetRequiredService<ILoggerFactory>()));

var authInProcess = false;
var appInProcess = false;

#if AUTH_INPROCESS
authInProcess = true;
services.AddAuthApiServices(isInProcess: true);
#endif

#if APP_INPROCESS
appInProcess = true;
services.AddAppApiServices(isInProcess: true);
#endif

// Configure YARP for API proxying (Auth/App when running out-of-process)
services.AddSingleton<IProxyConfigProvider>(
	new DynamicProxyConfigProvider(configuration, authInProcess, appInProcess));
services.AddReverseProxy()
	.AddServiceDiscoveryDestinationResolver()
	.AddTransforms(builderContext =>
	{
		builderContext.AddRequestTransform(async transformContext =>
		{
			var user = transformContext.HttpContext.User;
			if (user?.Identity?.IsAuthenticated != true)
				return;

			var sessionId = user.FindFirstValue("token_session_id");
			if (string.IsNullOrEmpty(sessionId))
				return;

			var tokenCache = transformContext.HttpContext.RequestServices
				.GetRequiredService<ITokenCacheService>();

			var accessToken = await tokenCache.GetAccessTokenAsync(sessionId);
			if (!string.IsNullOrEmpty(accessToken))
			{
				transformContext.ProxyRequest.Headers.Authorization =
					new AuthenticationHeaderValue("Bearer", accessToken);
			}
			else
			{
				// Token cache miss or refresh failed — force re-authentication
				transformContext.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
			}
		});
	});

// Register service clients (proxies)
#if AUTH_INPROCESS
//services.AddAuthClients(configuration, authInProcess);
#endif
#if APP_INPROCESS
//services.AddApp1Client(configuration, appInProcess);
#endif

builder.Services.AddApiVersioning(options =>
{
	options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
	options.GroupNameFormat = "'v'VVV";
	options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	IdentityModelEventSource.ShowPII = true;
	app.UseDeveloperExceptionPage();
	app.MapOpenApi();
}
else
{
	app.UseExceptionHandler("/api/error");
}

app.UseSecurityHeaders();

app.UseHttpsRedirection();
app.UseRouting();

// CORS must be called after UseRouting and before UseAuthentication
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

// Map API controllers
app.MapControllers();
app.MapNotFound("/api/{**segment}");

// Map health check endpoints (in development only)
app.MapDefaultEndpoints();

app.UseWhen(
	ctx => ctx.Request.Path.StartsWithSegments("/api/auth")
		|| ctx.Request.Path.StartsWithSegments("/api/app"),
	branch =>
	{
		branch.Use(async (ctx, next) =>
		{
			// breakpoint here
			await next();
		});
	});

// Map YARP reverse proxy for Auth/App APIs when running out-of-process
app.MapReverseProxy();

app.Run();
