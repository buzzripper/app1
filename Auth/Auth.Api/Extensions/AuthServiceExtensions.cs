using Dyvenix.Auth.Api.Services;
using Dyvenix.Auth.Shared.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace Dyvenix.Auth.Api.Extensions;

public static class AuthServiceExtensions
{
	/// <summary>
	/// Registers Auth API services and controllers.
	/// Call this when hosting Auth services (standalone or in-process).
	/// </summary>
	public static IServiceCollection AddAuthApiServices(this IServiceCollection services, bool isInProcess)
	{
		// Register business logic services
		services.AddScoped<ISystemService, SystemService>();
		services.AddScoped<ITokenEnrichmentService, TokenEnrichmentService>();

		if (!isInProcess)
		{
			// Add Controllers
			services.AddControllers();

			// Add OpenAPI support
			services.AddOpenApi();
		}

		return services;
	}

	/// <summary>
	/// Maps OpenAPI and Scalar API documentation endpoints for Auth API.
	/// Call this in development or when you want to expose API documentation.
	/// </summary>
	public static IEndpointRouteBuilder MapAuthApiDocumentation(this IEndpointRouteBuilder app)
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
