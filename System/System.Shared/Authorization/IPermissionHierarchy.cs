using System.Collections.Generic;

namespace App1.System.Shared.Authorization;

/// <summary>
/// Provides permission hierarchy lookup for a module.
/// Each module implements this to define which permissions imply others.
/// </summary>
public interface IPermissionHierarchy
{
	/// <summary>
	/// Returns permissions that imply the requested permission, or empty if none.
	/// </summary>
	IReadOnlyList<string> GetImplyingPermissions(string permission);
}
