#if AUTH_INPROCESS
using App1.Auth.Api.Extensions;
using App1.Auth.Shared.Authorization;
#endif
#if APP_INPROCESS
using App1.App.Api.Extensions;
using App1.App.Shared.Authorization;
#endif
using App1.App.Shared.Extensions;
using App1.Auth.Shared.Extensions;
using App1.System.Apis.Extensions;
using App1.System.Shared.Authorization;
using App1.System.Shared.Context;
using App1.Portal.Server;
using App1.Portal.Server.Interfaces;
using App1.Portal.Server.Filters;
using App1.Portal.Server.Services;
using App1.Portal.Server.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add OpenTelemetry support
builder.ConfigureOpenTelemetry();
builder.AddDefaultHealthChecks();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
	serverOptions.AddServerHeader = false;
});

builder.Services.AddOpenApi();

var services = builder.Services;
var configuration = builder.Configuration;

// Add request context for user/org access across all tiers
services.AddRequestContext();

// Add permission-based authorization (for in-process scenarios)
services.AddPermissionAuthorization();
#if AUTH_INPROCESS
services.AddAuthAuthorization();
#endif
#if APP_INPROCESS
services.AddAppAuthorization();
#endif

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
services.AddScoped<PortalExceptionFilter<PortalSystemService>>();

services.AddHttpClient();
services.AddOptions();

// Get scopes for downstream API token acquisition
var scopes = configuration.GetValue<string>("DownstreamApi:Scopes");
string[] initialScopes = scopes?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [];

// Build Entra ID configuration
var entraIdSettings = configuration.GetSection("MicrosoftEntraID");
var instance = entraIdSettings["Instance"] ?? throw new InvalidOperationException("MicrosoftEntraID:Instance is required");
var tenantId = entraIdSettings["TenantId"] ?? throw new InvalidOperationException("MicrosoftEntraID:TenantId is required");
var clientId = entraIdSettings["ClientId"] ?? throw new InvalidOperationException("MicrosoftEntraID:ClientId is required");
var authority = $"{instance.TrimEnd('/')}/{tenantId}/v2.0";

// Configure Microsoft Entra ID authentication (cookie-based for browser clients)
// Used by:
// - Portal's AccountController (explicit Cookie scheme)
// - In-process API controllers (via dual auth policy)
services.AddMicrosoftIdentityWebAppAuthentication(configuration, "MicrosoftEntraID")
	.EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
	.AddInMemoryTokenCaches();

// Add JWT Bearer authentication
// Used by:
// - Direct API access (Postman, mobile apps, 3rd-party)
// - In-process API controllers (via dual auth policy)
// - Out-of-process API calls (via YARP JWT transform)
services.AddAuthentication()
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
	{
		options.Authority = authority;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidAudience = clientId,
			ValidIssuer = authority
		};

		// Configure JWT Bearer to return 401 for API calls (no redirects)
		options.Events = new JwtBearerEvents
		{
			OnAuthenticationFailed = context =>
			{
				var logger = context.HttpContext.RequestServices
					.GetService<ILogger<JwtBearerEvents>>();
				logger?.LogWarning(context.Exception, "JWT authentication failed");
				return Task.CompletedTask;
			},
			OnChallenge = context =>
			{
				// Suppress default redirect behavior
				context.HandleResponse();

				// Return 401 with JSON response for APIs
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				context.Response.ContentType = "application/json";

				var result = System.Text.Json.JsonSerializer.Serialize(new
				{
					error = "unauthorized",
					error_description = "Authentication required. Provide a valid Bearer token in the Authorization header."
				});

				return context.Response.WriteAsync(result);
			}
		};
	});

// Configure OpenID Connect to save tokens (including id_token) for logout
services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
	options.SaveTokens = true; // Required for id_token_hint in logout
});

// If using downstream APIs and in memory cache, you need to reset the cookie session if the cache is missing
if (initialScopes.Length > 0)
{
	services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme,
		options => options.Events = new RejectSessionCookieWhenAccountNotInCacheEvents(initialScopes));
}

// Configure cookie settings for cross-origin Angular app
services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
	options.Cookie.SameSite = SameSiteMode.Lax;
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Configure authorization policies
// 
// HYBRID ARCHITECTURE:
// 
// In-Process API Controllers (Auth/App running in Portal.Server):
//   - Accept Cookie OR JWT Bearer (dual auth)
//   - Angular → Cookie auth works
//   - Postman → JWT Bearer works
// 
// Out-of-Process API Controllers (Auth/App running separately):
//   - Routes handled by YARP
//   - YARP JWT transform converts cookie → JWT Bearer
//   - Downstream service only sees JWT Bearer
// 
// Portal AccountController:
//   - Cookie auth only (explicit scheme attribute)
//   - Handles login/logout redirects
services.AddAuthorization(options =>
{
	// Default policy accepts BOTH Cookie and JWT Bearer
	// This enables in-process controllers to work with Angular (cookie) AND Postman (JWT)
	options.DefaultPolicy = new AuthorizationPolicyBuilder()
		.RequireAuthenticatedUser()
		.AddAuthenticationSchemes(
			CookieAuthenticationDefaults.AuthenticationScheme,
			JwtBearerDefaults.AuthenticationScheme)
		.Build();
});

// Register Portal's own controllers (AccountController, SystemController)
services.AddControllers();

services.AddTransient<IPortalModuleLogger>(sp => new PortalModuleLogger(sp.GetRequiredService<ILoggerFactory>()));

#if AUTH_INPROCESS
var authInProcess = true;
services.AddAuthApiServices(isInProcess: true);
#else
var authInProcess = false;
#endif

#if APP_INPROCESS
var appInProcess = true;
services.AddAppApiServices(isInProcess: true);
#else
var appInProcess = false;
#endif

// Configure YARP reverse proxy
// 
// For IN-PROCESS modules:
//   - Routes are SKIPPED by DynamicProxyConfigProvider
//   - Controllers accessed directly via MapControllers()
//   - Dual auth (Cookie + JWT Bearer) handles authentication
// 
// For OUT-OF-PROCESS modules:
//   - YARP routes to remote service
//   - JWT transform converts cookie → JWT Bearer token
//   - Downstream service receives JWT Bearer token only
services.AddReverseProxy()
	.LoadFromConfig(configuration.GetSection("ReverseProxy"))
	.AddJwtForwardingTransform(initialScopes);

// Register service clients (proxies) for non-HTTP service calls
services.AddAuthClients(configuration, authInProcess);
services.AddApp1Client(configuration, appInProcess);

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

// ROUTING:
// 
// MapControllers() handles:
//   - Portal's own controllers (/api/Account/*, /api/portal/*)
//   - In-process Auth controllers (/api/auth/*) if AUTH_INPROCESS
//   - In-process App controllers (/api/app/*) if APP_INPROCESS
// 
// MapReverseProxy() handles:
//   - Out-of-process Auth routes (/api/auth/*) if NOT AUTH_INPROCESS
//   - Out-of-process App routes (/api/app/*) if NOT APP_INPROCESS
//   - Routes are dynamically filtered by DynamicProxyConfigProvider
app.MapControllers();
app.MapNotFound("/api/{**segment}");

// Map health check endpoints
app.MapDefaultEndpoints();

// Map YARP reverse proxy (only active for out-of-process modules)
app.MapReverseProxy();

app.Run();
