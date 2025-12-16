using System;
using System.Collections.Generic;
using App1.System.Shared.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace App1.App.Shared.Authorization;

/// <summary>
/// Permission constants for the App module.
/// Format: app.{resource}.{action}
/// </summary>
public static class AppPermissions
{
	// Orders
	public const string OrdersRead = "app.orders.read";
	public const string OrdersWrite = "app.orders.write";
	public const string OrdersFull = "app.orders.full";

	// Products
	public const string ProductsRead = "app.products.read";
	public const string ProductsWrite = "app.products.write";
	public const string ProductsFull = "app.products.full";

	// Admin (implies all App permissions)
	public const string AdminFull = "app.admin.full";

	/// <summary>
	/// All App module permissions for policy registration.
	/// </summary>
	public static readonly string[] All =
	[
		OrdersRead, OrdersWrite, OrdersFull,
		ProductsRead, ProductsWrite, ProductsFull,
		AdminFull
	];
}

/// <summary>
/// Defines the permission hierarchy for the App module.
/// Higher permissions (write, full, admin) imply lower ones (read).
/// </summary>
public class AppPermissionHierarchy : IPermissionHierarchy
{
	private static readonly Dictionary<string, string[]> ImpliedBy = new(StringComparer.OrdinalIgnoreCase)
	{
		[AppPermissions.OrdersRead] = [AppPermissions.OrdersWrite, AppPermissions.OrdersFull, AppPermissions.AdminFull],
		[AppPermissions.OrdersWrite] = [AppPermissions.OrdersFull, AppPermissions.AdminFull],
		[AppPermissions.OrdersFull] = [AppPermissions.AdminFull],

		[AppPermissions.ProductsRead] = [AppPermissions.ProductsWrite, AppPermissions.ProductsFull, AppPermissions.AdminFull],
		[AppPermissions.ProductsWrite] = [AppPermissions.ProductsFull, AppPermissions.AdminFull],
		[AppPermissions.ProductsFull] = [AppPermissions.AdminFull],
	};

	public IReadOnlyList<string> GetImplyingPermissions(string permission)
	{
		return ImpliedBy.TryGetValue(permission, out var implying) ? implying : [];
	}
}

public static class AppAuthorizationExtensions
{
	/// <summary>
	/// Adds App module permission hierarchy and policies.
	/// </summary>
	public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
	{
		services.AddPermissionHierarchy<AppPermissionHierarchy>();

		services.AddAuthorizationBuilder()
			.AddPermissionPolicy(AppPermissions.OrdersRead)
			.AddPermissionPolicy(AppPermissions.OrdersWrite)
			.AddPermissionPolicy(AppPermissions.OrdersFull)
			.AddPermissionPolicy(AppPermissions.ProductsRead)
			.AddPermissionPolicy(AppPermissions.ProductsWrite)
			.AddPermissionPolicy(AppPermissions.ProductsFull)
			.AddPermissionPolicy(AppPermissions.AdminFull);

		return services;
	}
}
