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
using Yarp.ReverseProxy.Configuration;

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
.AddServiceDiscoveryDestinationResolver();

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
