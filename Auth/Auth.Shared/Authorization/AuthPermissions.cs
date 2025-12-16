using System;
using System.Collections.Generic;
using App1.System.Shared.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace App1.Auth.Shared.Authorization;

/// <summary>
/// Permission constants for the Auth module.
/// Format: auth.{resource}.{action}
/// </summary>
public static class AuthPermissions
{
	// Users
	public const string UsersRead = "auth.users.read";
	public const string UsersWrite = "auth.users.write";
	public const string UsersFull = "auth.users.full";

	// Roles
	public const string RolesRead = "auth.roles.read";
	public const string RolesWrite = "auth.roles.write";
	public const string RolesFull = "auth.roles.full";

	// Admin (implies all Auth permissions)
	public const string AdminFull = "auth.admin.full";

	/// <summary>
	/// All Auth module permissions for policy registration.
	/// </summary>
	public static readonly string[] All =
	[
		UsersRead, UsersWrite, UsersFull,
		RolesRead, RolesWrite, RolesFull,
		AdminFull
	];
}

/// <summary>
/// Defines the permission hierarchy for the Auth module.
/// Higher permissions (write, full, admin) imply lower ones (read).
/// </summary>
public class AuthPermissionHierarchy : IPermissionHierarchy
{
	private static readonly Dictionary<string, string[]> ImpliedBy = new(StringComparer.OrdinalIgnoreCase)
	{
		[AuthPermissions.UsersRead] = [AuthPermissions.UsersWrite, AuthPermissions.UsersFull, AuthPermissions.AdminFull],
		[AuthPermissions.UsersWrite] = [AuthPermissions.UsersFull, AuthPermissions.AdminFull],
		[AuthPermissions.UsersFull] = [AuthPermissions.AdminFull],

		[AuthPermissions.RolesRead] = [AuthPermissions.RolesWrite, AuthPermissions.RolesFull, AuthPermissions.AdminFull],
		[AuthPermissions.RolesWrite] = [AuthPermissions.RolesFull, AuthPermissions.AdminFull],
		[AuthPermissions.RolesFull] = [AuthPermissions.AdminFull],
	};

	public IReadOnlyList<string> GetImplyingPermissions(string permission)
	{
		return ImpliedBy.TryGetValue(permission, out var implying) ? implying : [];
	}
}

public static class AuthAuthorizationExtensions
{
	/// <summary>
	/// Adds Auth module permission hierarchy and policies.
	/// </summary>
	public static IServiceCollection AddAuthAuthorization(this IServiceCollection services)
	{
		services.AddPermissionHierarchy<AuthPermissionHierarchy>();

		services.AddAuthorizationBuilder()
			.AddPermissionPolicy(AuthPermissions.UsersRead)
			.AddPermissionPolicy(AuthPermissions.UsersWrite)
			.AddPermissionPolicy(AuthPermissions.UsersFull)
			.AddPermissionPolicy(AuthPermissions.RolesRead)
			.AddPermissionPolicy(AuthPermissions.RolesWrite)
			.AddPermissionPolicy(AuthPermissions.RolesFull)
			.AddPermissionPolicy(AuthPermissions.AdminFull);

		return services;
	}
}
