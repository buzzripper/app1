using Dyvenix.App1.App.Api.Services;
using Dyvenix.App1.App.Endpoints.v1;
using Dyvenix.App1.App.Shared.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace Dyvenix.App1.App.Api.Extensions;

public static partial class ServiceCollectionExt
{
	// Declaration of partial method for code-generated services
	static partial void AddGeneratedServices(IServiceCollection services);

	/// <summary>
	/// Registers App API services.
	/// Call this when hosting App services (standalone or in-process).
	/// </summary>
	public static IServiceCollection AddAppApiServices(this IServiceCollection services, bool isInProcess)
	{
		// Register business logic services
		services.AddScoped<IAppSystemService, AppSystemService>();

		if (!isInProcess)
		{
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
	public static IEndpointRouteBuilder MapAppApiDocumentation(this IEndpointRouteBuilder app)
	{
		app.MapPatientEndpoints();

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
