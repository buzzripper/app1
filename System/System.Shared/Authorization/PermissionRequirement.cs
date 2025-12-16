using Microsoft.AspNetCore.Authorization;

namespace App1.System.Shared.Authorization;

/// <summary>
/// Authorization requirement that checks for a specific permission.
/// </summary>
public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
	public string Permission { get; } = permission;
}
