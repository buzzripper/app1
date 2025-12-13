#if AUTH_INPROCESS
using Dyvenix.Auth.Api.Extensions;
#endif
#if APP_INPROCESS
using Dyvenix.App.Api.Extensions;
#endif
using Dyvenix.App1.Portal.Server;
using Dyvenix.App1.Portal.Server.Interfaces;
using Dyvenix.App1.Portal.Server.Services;
using Yarp.ReverseProxy.Configuration;
using Dyvenix.App.Shared.Extensions;
using Dyvenix.Auth.Shared.Extensions;
using Dyvenix.System.Apis.Extensions;

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

services.AddSecurityHeaderPolicies()
	.SetPolicySelector(ctx =>
	{
		if (ctx.HttpContext.Request.Path.StartsWithSegments("/api"))
		{
			return ApiSecurityHeadersDefinitions.GetHeaderPolicyCollection(builder.Environment.IsDevelopment());
		}

		return SecurityHeadersDefinitions.GetHeaderPolicyCollection(
		  builder.Environment.IsDevelopment(),
		  configuration["MicrosoftEntraID:Instance"]);
	});

services.AddAntiforgery(options =>
{
	options.HeaderName = "X-XSRF-TOKEN";
	options.Cookie.Name = "__Host-X-XSRF-TOKEN";
	options.Cookie.SameSite = SameSiteMode.Strict;
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

services.AddScoped<ISystemService, SystemService>();
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

services.AddControllersWithViewsAndExceptionHandling()
	.AddMvcOptions(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

services.AddRazorPages().AddMvcOptions(options =>
{
	//var policy = new AuthorizationPolicyBuilder()
	//    .RequireAuthenticatedUser()
	//    .Build();
	//options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();

#if AUTH_INPROCESS
	var authInProcess = true;
	services.AddAuthApiServices(isInProcess: true);
#else
	var authInProcess = false;
#endif

#if APP_INPROCESS
	var appInProcess = true;
	services.AddAppApiServices();
#else
	var appInProcess = false;
#endif

// Configure YARP with dynamic config based on compile-time defines
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
	app.UseWebAssemblyDebugging();
	app.MapOpenApi();
}
else
{
	app.UseExceptionHandler("/Error");
}

app.UseSecurityHeaders();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Map specific endpoints first (highest priority)
app.MapRazorPages();
app.MapControllers();
app.MapNotFound("/api/{**segment}");

// Map health check endpoints (in development only)
app.MapDefaultEndpoints();

// Map YARP reverse proxy for UI dev server (only in development)
if (app.Environment.IsDevelopment())
{
	var uiDevServer = app.Configuration.GetValue<string>("UiDevServerUrl");
	if (!string.IsNullOrEmpty(uiDevServer))
	{
		// YARP routes should be mapped before the fallback
		// They have specific paths that will match before the fallback
		app.MapReverseProxy();
	}
}

// Fallback must be LAST - catches everything that didn't match above
app.MapFallbackToPage("/_Host");

app.Run();
