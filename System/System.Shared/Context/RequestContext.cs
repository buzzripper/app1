using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace App1.System.Shared.Context;

/// <summary>
/// Scoped implementation that provides access to caller context from HttpContext.User.
/// All properties are eagerly loaded once during construction.
/// </summary>
public class RequestContext : IRequestContext
{
	public string? UserId { get; }
	public string? OrganizationId { get; }
	public string? DisplayName { get; }
	public string? Email { get; }
	public bool IsAuthenticated { get; }
	public bool IsIntegration { get; }
	public string? IntegrationClientId { get; }
	public IReadOnlyList<string> Permissions { get; }

	public RequestContext(IHttpContextAccessor httpContextAccessor)
	{
		var user = httpContextAccessor.HttpContext?.User;

		IsAuthenticated = user?.Identity?.IsAuthenticated ?? false;

		if (!IsAuthenticated || user == null)
		{
			Permissions = [];
			return;
		}

		UserId = user.FindFirst("oid")?.Value ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		OrganizationId = user.FindFirst("tid")?.Value ?? user.FindFirst("org_id")?.Value;
		DisplayName = user.FindFirst("name")?.Value ?? user.FindFirst(ClaimTypes.Name)?.Value;
		Email = user.FindFirst("email")?.Value ?? user.FindFirst("preferred_username")?.Value;

		// Determine if this is an integration (app-only) token
		var idTypeClaim = user.FindFirst("idtyp");
		IsIntegration = idTypeClaim?.Value == "app"
			|| (user.HasClaim(c => c.Type == "roles") && !user.HasClaim(c => c.Type == "scp"));

		IntegrationClientId = IsIntegration
			? user.FindFirst("azp")?.Value ?? user.FindFirst("appid")?.Value ?? user.FindFirst("client_id")?.Value
			: null;

		// Load all permissions once
		Permissions = ExtractPermissions(user);
	}

	public bool HasPermission(string permission) =>
		Permissions.Contains(permission, StringComparer.OrdinalIgnoreCase);

	private static List<string> ExtractPermissions(ClaimsPrincipal user)
	{
		var permissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// 'perm' claims (custom permission claims)
		foreach (var claim in user.FindAll("perm"))
		{
			foreach (var p in claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries))
				permissions.Add(p);
		}

		// 'roles' claims (Entra ID app roles) - only if they look like permissions (contain dots)
		foreach (var claim in user.FindAll("roles"))
		{
			if (claim.Value.Contains('.'))
				permissions.Add(claim.Value);
		}

		// 'scp' claims (delegated scopes) - only if they look like permissions (contain dots)
		foreach (var claim in user.FindAll("scp"))
		{
			foreach (var scope in claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries))
			{
				if (scope.Contains('.'))
					permissions.Add(scope);
			}
		}

		return [.. permissions];
	}
}
