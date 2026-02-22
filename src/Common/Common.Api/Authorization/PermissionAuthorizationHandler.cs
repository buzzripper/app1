using Microsoft.AspNetCore.Authorization;

namespace Dyvenix.App1.Common.Api.Authorization;

public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
	{
		if (context.User?.Identity?.IsAuthenticated != true)
			return Task.CompletedTask;

		var permissions = context.User.FindAll("perm")
			.SelectMany(claim => claim.Value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
			.ToHashSet(StringComparer.OrdinalIgnoreCase);

		if (permissions.Count == 0)
			return Task.CompletedTask;

		if (requirement.Permissions.Any(permission => permissions.Contains(permission)))
			context.Succeed(requirement);

		return Task.CompletedTask;
	}
}
