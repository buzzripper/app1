namespace App1.System.Shared.Context;

/// <summary>
/// Provides access to the current request's caller context.
/// Available via DI in all tiers (controllers, services, repositories).
/// </summary>
public interface IRequestContext
{
	/// <summary>The authenticated user's ID (from 'oid' or NameIdentifier claim)</summary>
	string? UserId { get; }

	/// <summary>The organization/tenant ID (from 'tid' or 'org_id' claim)</summary>
	string? OrganizationId { get; }

	/// <summary>User's display name</summary>
	string? DisplayName { get; }

	/// <summary>User's email address</summary>
	string? Email { get; }

	/// <summary>True if the request is authenticated</summary>
	bool IsAuthenticated { get; }

	/// <summary>True if this is a machine-to-machine integration call (client_credentials flow)</summary>
	bool IsIntegration { get; }

	/// <summary>The integration client ID (when IsIntegration is true)</summary>
	string? IntegrationClientId { get; }

	/// <summary>All permissions the caller has (from 'perm' and 'roles' claims)</summary>
	global::System.Collections.Generic.IReadOnlyList<string> Permissions { get; }

	/// <summary>Check if the caller has a specific permission</summary>
	bool HasPermission(string permission);
}
