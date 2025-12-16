#if AUTH_INPROCESS
using App1.Auth.Api.Extensions;
#endif
#if APP_INPROCESS
using App1.App.Api.Extensions;
#endif
using Yarp.ReverseProxy.Configuration;
using App1.App.Shared.Extensions;
using App1.Auth.Shared.Extensions;
using App1.System.Apis.Extensions;
using App1.System.Shared.Context;
using App1.Portal.Server;
using App1.Portal.Server.Interfaces;
using App1.Portal.Server.Filters;
using App1.Portal.Server.Services;
using App1.Portal.Server.Logging;

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

var scopes = configuration.GetValue<string>("DownstreamApi:Scopes");
string[] initialScopes = scopes?.Split(' ') ?? Array.Empty<string>();

services.AddMicrosoftIdentityWebAppAuthentication(configuration, "MicrosoftEntraID")
	.EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
	.AddInMemoryTokenCaches();

// Configure OpenID Connect to save tokens (including id_token) for logout
services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
	options.SaveTokens = true; // Required for id_token_hint in logout
});

// If using downstream APIs and in memory cache, you need to reset the cookie session if the cache is missing
// If you use persistent cache, you do not require this.
// You can also return the 403 with the required scopes, this needs special handling for ajax calls
// The check is only for single scopes
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

// Configure YARP for API proxying (Auth/App when running out-of-process)
services.AddSingleton<IProxyConfigProvider>(
	new DynamicProxyConfigProvider(configuration, authInProcess, appInProcess));
services.AddReverseProxy();

// Register service clients (proxies)
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

// Map API controllers
app.MapControllers();
app.MapNotFound("/api/{**segment}");

// Map health check endpoints (in development only)
app.MapDefaultEndpoints();

// Map YARP reverse proxy for Auth/App APIs when running out-of-process
app.MapReverseProxy();

app.Run();
