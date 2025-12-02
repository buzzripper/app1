using Dyvenix.App1.Api.Services;
using Dyvenix.App1.Shared.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace Dyvenix.App1.Api.Extensions;

public static class App1ServiceExtensions
{
	/// <summary>
	/// Registers App1 API services and controllers.
	/// Call this when hosting App1 services (standalone or in-process).
	/// </summary>
	public static IServiceCollection AddApp1ApiServices(this IServiceCollection services)
	{
		// Register business logic services
		services.AddScoped<IApp1SystemService, SystemService>();

		// Add Controllers
		services.AddControllers();

		// Add OpenAPI support
		services.AddOpenApi();

		return services;
	}

	/// <summary>
	/// Maps App1 controllers to the application with optional path prefix.
	/// Call this when hosting App1 services (standalone or in-process).
	/// </summary>
	/// <param name="app">The endpoint route builder</param>
	/// <param name="pathPrefix">Optional path prefix (e.g., "/api/app1"). Use empty string for no prefix.</param>
	public static IEndpointRouteBuilder MapApp1Endpoints(this IEndpointRouteBuilder app, string pathPrefix = "")
	{
		if (string.IsNullOrEmpty(pathPrefix))
		{
			// No prefix - map controllers directly
			app.MapControllers();
		}
		else
		{
			// Apply prefix using MapGroup
			var group = app.MapGroup(pathPrefix);
			group.MapControllers();
		}

		return app;
	}

	/// <summary>
	/// Maps Scalar API documentation UI for App1 endpoints.
	/// Call this in development or when you want to expose API documentation.
	/// </summary>
	public static IEndpointRouteBuilder MapApp1ApiDocumentation(this IEndpointRouteBuilder app)
	{
		app.MapOpenApi();
		app.MapScalarApiReference(options =>
		{
			options
				.WithTitle("App1 API")
				.WithTheme(ScalarTheme.Purple)
				.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
		});

		return app;
	}
}
