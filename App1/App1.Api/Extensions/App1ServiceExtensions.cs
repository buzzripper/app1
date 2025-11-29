using App1.Api.Endpoints;
using App1.Api.Services;
using App1.Shared.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace App1.Api.Extensions;

public static class App1ServiceExtensions
{
    /// <summary>
    /// Registers App1 API services and endpoints.
    /// Call this when hosting App1 services (standalone or in-process).
    /// </summary>
    public static IServiceCollection AddApp1ApiServices(this IServiceCollection services)
    {
        // Register business logic services
        services.AddScoped<ISystemService, SystemService>();
        
        // Register endpoint modules
        services.AddSingleton<IEndpointModule, SystemEndpointModule>();
        
        // Add OpenAPI support
        services.AddOpenApi();
        
        return services;
    }

    /// <summary>
    /// Maps App1 endpoints to the application.
    /// Call this when hosting App1 services (standalone or in-process).
    /// </summary>
    public static IEndpointRouteBuilder MapApp1Endpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.ServiceProvider.GetServices<IEndpointModule>();
        
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoints(app);
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
