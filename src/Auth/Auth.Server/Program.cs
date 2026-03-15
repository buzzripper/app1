using Dyvenix.App1.App.Shared.ApiClients.v1;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Api.Extensions;
using Dyvenix.App1.Auth.Data;
using Dyvenix.App1.Auth.Data.Context;
using Dyvenix.App1.Auth.Data.Fido2;
using Dyvenix.App1.Auth.Server;
using Dyvenix.App1.Auth.Server.Fido2;
using Dyvenix.App1.Auth.Server.Services;
using Dyvenix.App1.Auth.Shared.Authorization;
using Dyvenix.App1.Common.Api.Authorization;
using Dyvenix.App1.Common.Api.Extensions;
using Dyvenix.App1.Common.Server;
using Fido2NetLib;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Quartz;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (telemetry, health checks, resilience)
// NOTE: Requires Aspire AppHost orchestration; comment out for standalone runs.
//builder.AddServiceDefaults();

// Register health check services (normally done by AddServiceDefaults)
builder.AddDefaultHealthChecks();

// Configure OpenTelemetry logging
builder.ConfigureOpenTelemetry();

// MVC + Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Tenant context (scoped per request, set by TenantResolutionMiddleware)
builder.Services.AddScoped<ITenantContext, TenantContext>();

// Entity Framework + SQL Server + OpenIddict entity sets
builder.Services.AddDbContext<AuthDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
		b => b.MigrationsAssembly("Dyvenix.App1.Auth.Data"));
	options.UseOpenIddict();
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
	.AddEntityFrameworkStores<AuthDbContext>()
	.AddDefaultTokenProviders()
	.AddDefaultUI()
	.AddTokenProvider<Fido2UserTwoFactorTokenProvider>("FIDO2");

//// FIDO2/WebAuthn
builder.Services.Configure<Fido2Configuration>(builder.Configuration.GetSection("fido2"));
builder.Services.AddScoped<Fido2Store>();

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(2);
	options.Cookie.HttpOnly = true;
	options.Cookie.SameSite = SameSiteMode.None;
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Identity options — use OpenIddict JWT claim types
builder.Services.Configure<IdentityOptions>(options =>
{
	options.ClaimsIdentity.UserNameClaimType = Claims.Name;
	options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
	options.ClaimsIdentity.RoleClaimType = Claims.Role;
	options.ClaimsIdentity.EmailClaimType = Claims.Email;
	options.SignIn.RequireConfirmedAccount = false;
});

// Quartz.NET for scheduled tasks (OpenIddict token pruning)
//builder.Services.AddQuartz(options =>
//{
//	options.UseSimpleTypeLoader();
//	options.UseInMemoryStore();
//});
//builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

// CORS — driven by AllowedHosts in appsettings.json
var allowedHosts = builder.Configuration["AllowedHosts"] ?? "*";
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAllOrigins", policy =>
	{
		policy
			.AllowCredentials()
			.SetIsOriginAllowed(origin =>
			{
				if (allowedHosts == "*")
					return true;

				if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
					return false;

				var host = uri.Host;
				return allowedHosts
					.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
					.Any(pattern =>
					{
						if (pattern.StartsWith("*."))
							return host.EndsWith(pattern[1..], StringComparison.OrdinalIgnoreCase);

						return string.Equals(host, pattern, StringComparison.OrdinalIgnoreCase);
					});
			})
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});

// Authentication — cookie-based default for Identity pages
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);

// Required so that dynamically-registered OpenIdConnect schemes (see Worker)
// get their StringDataFormat, Backchannel, etc. initialised by post-configure.
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<OpenIdConnectOptions>, OpenIdConnectPostConfigureOptions>());

// OpenIddict
builder.Services.AddOpenIddict()
	.AddCore(options =>
	{
		options.UseEntityFrameworkCore()
			   .UseDbContext<AuthDbContext>();
		//options.UseQuartz();
	})
	.AddServer(options =>
	{
		options.SetAuthorizationEndpointUris("connect/authorize")
			   .SetIntrospectionEndpointUris("connect/introspect")
			   .SetEndSessionEndpointUris("connect/logout")
			   .SetTokenEndpointUris("connect/token")
			   .SetUserInfoEndpointUris("connect/userinfo")
			   .SetEndUserVerificationEndpointUris("connect/verify");

		options.AllowAuthorizationCodeFlow()
			   .AllowHybridFlow()
			   .AllowClientCredentialsFlow()
			   .AllowRefreshTokenFlow();

		options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles, "app1-api", "integration-api");

		options.AddDevelopmentEncryptionCertificate()
			   .AddDevelopmentSigningCertificate();

		options.UseAspNetCore()
			   .EnableAuthorizationEndpointPassthrough()
			   .EnableEndSessionEndpointPassthrough()
			   .EnableTokenEndpointPassthrough()
			   .EnableUserInfoEndpointPassthrough()
			   .EnableStatusCodePagesIntegration();

		options.DisableAccessTokenEncryption();
	})
	.AddValidation(options =>
	{
		options.UseLocalServer();
		options.UseAspNetCore();
	});

// Worker (seeds tenants, users, registers external OIDC schemes and OpenIddict applications)
builder.Services.AddHostedService<Worker>();

// Auth.Api services (system endpoints, health, documentation)
builder.Services.AddPermissionAuthorization();
builder.Services.AddAuthApiServices(false);
builder.Services.AddBrandImgRepository(builder.Configuration);
builder.Services.AddStandardApiVersioning();

builder.Services.AddScoped<IClientRouter, ClientRouter>();
builder.Services.AddHttpClient<IClientService, ClientApiClient>();
builder.Services.AddHttpClient<IClientService, ClientApiClient>(client =>
{
	client.BaseAddress = new Uri("https://localhost:5003");
});

builder.Services.AddSingleton<PermissionRegistry>();

//----------------------------------------------------------------------------------------------

var app = builder.Build();

// Register this module's permissions
app.Services.GetRequiredService<PermissionRegistry>()
	.Register(AuthPermissions.Hierarchy);

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseMigrationsEndPoint();
}
else
{
	app.UseStatusCodePagesWithReExecute("~/error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowAllOrigins");

// Resolve tenant from acr_values before authentication
app.UseMiddleware<TenantResolutionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllers();
app.MapDefaultControllerRoute();
app.MapRazorPages();

// Auth.Api minimal API endpoints
app.MapEndpoints();
app.MapDefaultEndpoints();

// Enable API documentation in development
if (app.Environment.IsDevelopment())
	app.MapAuthApiDocumentation();

app.Run();
