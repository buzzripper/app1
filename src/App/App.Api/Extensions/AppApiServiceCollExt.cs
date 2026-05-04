using Dyvenix.App1.App.Api.Contracts.v1;
using Dyvenix.App1.App.Api.Endpoints.v1;
using Dyvenix.App1.App.Api.Services;
using Dyvenix.App1.App.Api.Services.v1;
using Dyvenix.App1.Auth.Shared.ApiClients.v1;
using Dyvenix.App1.Common.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace Dyvenix.App1.App.Api.Extensions;

public static partial class AppApiServiceCollExt
{
	// Declaration of partial methods for code-generated services
	static partial void AddGeneratedServices(IServiceCollection services);
	static partial void MapGeneratedEndpoints(IEndpointRouteBuilder app);
	static partial void AddDataServices(IServiceCollection services, IConfiguration configuration);

	/// <summary>
	/// Registers App API services.
	/// Call this when hosting App services (standalone or in-process).
	/// </summary>
	public static IServiceCollection AddAppApiServices(this IServiceCollection services, IConfiguration configuration, bool isInProcess)
	{
		services.AddCurrentUserServices();
		services.AddHealthChecks()
			.AddCheck<HealthService>("AD Agent Service Health");
		if (!isInProcess)
		{
			services.AddOpenApi();
		}

		// Register business logic services

		// Add code-generated services
		AddGeneratedServices(services);
		AddDataServices(services, configuration);

		// ClientAuth proxy service (calls Auth.Api via TenantApiClient)
		services.AddHttpContextAccessor();
		services.AddTransient<TenantApiBearerTokenHandler>();
		services.AddHttpClient<TenantApiClient>(client =>
		{
			client.BaseAddress = new Uri("https+http://auth-server");
		})
		.AddHttpMessageHandler<TenantApiBearerTokenHandler>();
		services.AddScoped<IClientAuthService, ClientAuthService>();

		return services;
	}

	/// <summary> 
	/// Maps endpoints
	/// </summary>
	public static IEndpointRouteBuilder MapAppEndpoints(this IEndpointRouteBuilder app)
	{
		app.MapClientAuthEndpoints();

		MapGeneratedEndpoints(app);

		return app;
	}

	/// <summary> 
	/// Maps OpenAPI and Scalar API documentation endpoints for Auth API.
	/// Call this in development or when you want to expose API documentation.
	/// </summary>
	public static IEndpointRouteBuilder MapAppApiDocumentation(this IEndpointRouteBuilder app)
	{
		app.MapOpenApi();
		app.MapScalarApiReference(options =>
		{
			options
				.WithTitle("Auth API")
				.WithTheme(ScalarTheme.DeepSpace);
		});

		return app;
	}


}
