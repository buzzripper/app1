using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using App1.System.Shared.Context;

namespace App1.System.Shared.Authorization;

/// <summary>
/// Handles permission-based authorization by checking the user's permissions
/// and any hierarchical permissions that imply the required permission.
/// </summary>
public class PermissionHandler(
	IRequestContext requestContext,
	IEnumerable<IPermissionHierarchy> hierarchies) : AuthorizationHandler<PermissionRequirement>
{
	protected override Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		PermissionRequirement requirement)
	{
		// Direct match
		if (requestContext.HasPermission(requirement.Permission))
		{
			context.Succeed(requirement);
			return Task.CompletedTask;
		}

		// Check all registered hierarchies for implying permissions
		foreach (var hierarchy in hierarchies)
		{
			var implyingPermissions = hierarchy.GetImplyingPermissions(requirement.Permission);
			foreach (var higherPermission in implyingPermissions)
			{
				if (requestContext.HasPermission(higherPermission))
				{
					context.Succeed(requirement);
					return Task.CompletedTask;
				}
			}
		}

		return Task.CompletedTask;
	}
}
