using Dyvenix.App1.Common.Api.Services;
using Dyvenix.App1.Common.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Dyvenix.App1.Common.Api.Extensions;

public static class CurrentUserServiceCollectionExtensions
{
	public static IServiceCollection AddCurrentUserServices(this IServiceCollection services)
	{
		services.AddHttpContextAccessor();
		services.AddScoped<ICurrentUserService, HttpContextCurrentUserService>();

		return services;
	}
}
