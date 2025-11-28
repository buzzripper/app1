using Auth.Api.Endpoints;
using Auth.Api.Services;
using Auth.Shared.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace Auth.Api.Extensions;

public static class AuthServiceExtensions
{
    /// <summary>
    /// Registers Auth API services and endpoints.
    /// Call this when hosting Auth services (standalone or in-process).
    /// </summary>
    public static IServiceCollection AddAuthApiServices(this IServiceCollection services)
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
    /// Maps Auth endpoints to the application.
    /// Call this when hosting Auth services (standalone or in-process).
    /// </summary>
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.ServiceProvider.GetServices<IEndpointModule>();
        
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoints(app);
        }
        
        return app;
    }

    /// <summary>
    /// Maps Scalar API documentation UI for Auth endpoints.
    /// Call this in development or when you want to expose API documentation.
    /// </summary>
    public static IEndpointRouteBuilder MapAuthApiDocumentation(this IEndpointRouteBuilder app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Auth API")
                .WithTheme(ScalarTheme.Purple)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
        
        return app;
    }
}
