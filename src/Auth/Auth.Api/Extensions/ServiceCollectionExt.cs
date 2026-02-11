using Dyvenix.App1.Auth.Api.Services;
using Dyvenix.App1.Auth.Shared.Interfaces;
using Dyvenix.App1.Common.Api.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace Dyvenix.App1.Auth.Api.Extensions;

public static partial class ServiceCollectionExt
{
	// Declaration of partial method for code-generated services
	static partial void AddGeneratedServices(IServiceCollection services);

	/// <summary>
	/// Registers Auth API services and controllers.
	/// Call this when hosting Auth services (standalone or in-process).
	/// </summary>
	public static IServiceCollection AddAuthApiServices(this IServiceCollection services, bool isInProcess)
	{
		// Register business logic services
		services.AddScoped<IAuthSystemService, AuthSystemService>();

		// Register exception filter for ServiceFilter attribute
		services.AddScoped<ApiExceptionFilter<AuthSystemService>>();

		if (!isInProcess)
		{
			// Add Controllers
			services.AddControllers();
			// Add OpenAPI support
			services.AddOpenApi();
		}

		// Add code-generated services
		AddGeneratedServices(services);

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
