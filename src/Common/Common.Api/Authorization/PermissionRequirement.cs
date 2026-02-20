using Microsoft.AspNetCore.Authorization;

namespace Dyvenix.App1.Common.Api.Authorization;

public sealed class PermissionRequirement : IAuthorizationRequirement
{
	public PermissionRequirement(IReadOnlyCollection<string> permissions)
	{
		Permissions = permissions;
	}

	public IReadOnlyCollection<string> Permissions { get; }
}
