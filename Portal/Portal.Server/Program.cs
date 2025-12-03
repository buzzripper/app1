#if AUTH_INPROCESS
using Dyvenix.Auth.Api.Extensions;
#endif
#if APP1_INPROCESS
using Dyvenix.App1.Api.Extensions;
#endif
using Dyvenix.App1.Portal.Server;
using Dyvenix.App1.Portal.Server.Services;
using Dyvenix.App1.Shared.Extensions;
using Dyvenix.Auth.Shared.Extensions;
using Yarp.ReverseProxy.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

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
		  configuration["OpenIddict:Authority"]);
	});

services.AddAntiforgery(options =>
{
	options.HeaderName = "X-XSRF-TOKEN";
	options.Cookie.Name = "__Host-X-XSRF-TOKEN";
	options.Cookie.SameSite = SameSiteMode.Strict;
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

services.AddHttpClient();
services.AddOptions();

// Configure OpenID Connect authentication with OpeniddictServer
services.AddAuthentication(options =>
{
	options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
	options.Cookie.Name = "__Host-portal-auth";
	options.Cookie.SameSite = SameSiteMode.Strict;
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
})
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
	options.Authority = configuration["OpenIddict:Authority"];
	options.ClientId = configuration["OpenIddict:ClientId"];
	options.ClientSecret = configuration["OpenIddict:ClientSecret"];
	options.ResponseType = OpenIdConnectResponseType.Code;
	options.ResponseMode = OpenIdConnectResponseMode.Query;
	
	options.Scope.Clear();
	options.Scope.Add("openid");
	options.Scope.Add("profile");
	options.Scope.Add("email");
	options.Scope.Add("offline_access");
	options.Scope.Add("dataEventRecords");
	
	options.SaveTokens = true;
	options.GetClaimsFromUserInfoEndpoint = true;
	options.RequireHttpsMetadata = true;
	
	options.TokenValidationParameters = new TokenValidationParameters
	{
		NameClaimType = "name",
		RoleClaimType = "role"
	};
});

services.AddControllersWithViews(options =>
	options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

services.AddRazorPages().AddMvcOptions(options =>
{
	//var policy = new AuthorizationPolicyBuilder()
	//    .RequireAuthenticatedUser()
	//    .Build();
	//options.Filters.Add(new AuthorizeFilter(policy));
});

#if AUTH_INPROCESS
	var authInProcess = true;
	services.AddAuthApiServices();
#else
	var authInProcess = false;
#endif

#if APP1_INPROCESS
	var app1InProcess = true;
	services.AddApp1ApiServices();
#else
	var app1InProcess = false;
#endif

// Configure YARP with dynamic config based on compile-time defines
services.AddSingleton<IProxyConfigProvider>(
	new DynamicProxyConfigProvider(configuration, authInProcess, app1InProcess));
services.AddReverseProxy();

// Register service clients (proxies)
services.AddAuthClients(configuration, authInProcess);
services.AddApp1Client(configuration, app1InProcess);

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
