using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace App1.System.Shared.Authorization;

public static class AuthorizationExtensions
{
	/// <summary>
	/// Adds the core permission-based authorization infrastructure.
	/// Call this once, then register module-specific hierarchies and policies.
	/// </summary>
	public static IServiceCollection AddPermissionAuthorization(this IServiceCollection services)
	{
		services.AddScoped<IAuthorizationHandler, PermissionHandler>();
		return services;
	}

	/// <summary>
	/// Registers a permission hierarchy for a module.
	/// </summary>
	public static IServiceCollection AddPermissionHierarchy<T>(this IServiceCollection services)
		where T : class, IPermissionHierarchy
	{
		services.AddSingleton<IPermissionHierarchy, T>();
		return services;
	}

	/// <summary>
	/// Adds a permission policy.
	/// </summary>
	public static AuthorizationBuilder AddPermissionPolicy(
		this AuthorizationBuilder builder,
		string permission)
	{
		builder.AddPolicy(permission, policy =>
			policy.AddRequirements(new PermissionRequirement(permission)));
		return builder;
	}
}
