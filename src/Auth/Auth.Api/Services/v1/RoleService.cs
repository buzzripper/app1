using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.DTOs.v1;
using Dyvenix.App1.Auth.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dyvenix.App1.Auth.Api.Services.v1;

public class RoleService : IRoleService
{
	private readonly RoleManager<IdentityRole> _roleManager;

	public RoleService(RoleManager<IdentityRole> roleManager)
	{
		_roleManager = roleManager;
	}

	#region Create

	public async Task<RoleDto> CreateRole(CreateRoleReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		var role = new IdentityRole(request.Name);

		var result = await _roleManager.CreateAsync(role);
		if (!result.Succeeded)
			throw new ValidationException(string.Join("; ", result.Errors.Select(e => e.Description)), []);

		return MapToDto(role);
	}

	#endregion

	#region Delete

	public async Task DeleteRole(string roleId)
	{
		if (string.IsNullOrEmpty(roleId))
			throw new ArgumentNullException(nameof(roleId));

		var role = await _roleManager.FindByIdAsync(roleId);
		if (role == null)
			throw new NotFoundException($"Role {roleId} not found");

		var result = await _roleManager.DeleteAsync(role);
		if (!result.Succeeded)
			throw new ValidationException(string.Join("; ", result.Errors.Select(e => e.Description)), []);
	}

	#endregion

	#region Update

	public async Task UpdateRole(UpdateRoleReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		var role = await _roleManager.FindByIdAsync(request.Id);
		if (role == null)
			throw new NotFoundException($"Role {request.Id} not found");

		role.Name = request.Name;

		var result = await _roleManager.UpdateAsync(role);
		if (!result.Succeeded)
			throw new ValidationException(string.Join("; ", result.Errors.Select(e => e.Description)), []);
	}

	#endregion

	#region Read

	public async Task<RoleDto> GetRoleById(string roleId)
	{
		if (string.IsNullOrEmpty(roleId))
			throw new ArgumentNullException(nameof(roleId));

		var role = await _roleManager.FindByIdAsync(roleId);
		if (role == null)
			throw new NotFoundException($"Role {roleId} not found");

		return MapToDto(role);
	}

	public async Task<RoleDto> GetRoleByName(string roleName)
	{
		if (string.IsNullOrEmpty(roleName))
			throw new ArgumentNullException(nameof(roleName));

		var role = await _roleManager.FindByNameAsync(roleName);
		if (role == null)
			throw new NotFoundException($"Role '{roleName}' not found");

		return MapToDto(role);
	}

	public async Task<List<RoleDto>> GetAllRoles()
	{
		var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
		return roles.Select(MapToDto).ToList();
	}

	#endregion

	#region Private Helpers

	private static RoleDto MapToDto(IdentityRole role)
	{
		return new RoleDto
		{
			Id = role.Id,
			Name = role.Name ?? string.Empty
		};
	}

	#endregion
}
