using Microsoft.Extensions.DependencyInjection;

namespace App1.System.Shared.Context;

public static class RequestContextExtensions
{
	/// <summary>
	/// Adds IRequestContext for accessing user/org context across all application tiers.
	/// </summary>
	public static IServiceCollection AddRequestContext(this IServiceCollection services)
	{
		services.AddHttpContextAccessor();
		services.AddScoped<IRequestContext, RequestContext>();
		return services;
	}
}
