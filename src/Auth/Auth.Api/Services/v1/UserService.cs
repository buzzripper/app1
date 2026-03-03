using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.DTOs.v1;
using Dyvenix.App1.Auth.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Dyvenix.App1.Auth.Api.Services.v1;

public class UserService : IUserService
{
	private readonly UserManager<IdentityUser> _userManager;

	public UserService(UserManager<IdentityUser> userManager)
	{
		_userManager = userManager;
	}

	#region Create

	public async Task<UserDto> CreateUser(CreateUserReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		var user = new IdentityUser
		{
			UserName = request.UserName,
			Email = request.Email,
			PhoneNumber = request.PhoneNumber
		};

		var result = await _userManager.CreateAsync(user, request.Password);
		if (!result.Succeeded)
			throw new ValidationException(string.Join("; ", result.Errors.Select(e => e.Description)), []);

		return MapToDto(user);
	}

	#endregion

	#region Delete

	public async Task DeleteUser(string userId)
	{
		if (string.IsNullOrEmpty(userId))
			throw new ArgumentNullException(nameof(userId));

		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
			throw new NotFoundException($"User {userId} not found");

		var result = await _userManager.DeleteAsync(user);
		if (!result.Succeeded)
			throw new ValidationException(string.Join("; ", result.Errors.Select(e => e.Description)), []);
	}

	#endregion

	#region Update

	public async Task UpdateUser(UpdateUserReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		var user = await _userManager.FindByIdAsync(request.Id);
		if (user == null)
			throw new NotFoundException($"User {request.Id} not found");

		user.UserName = request.UserName;
		user.Email = request.Email;
		user.PhoneNumber = request.PhoneNumber;

		var result = await _userManager.UpdateAsync(user);
		if (!result.Succeeded)
			throw new ValidationException(string.Join("; ", result.Errors.Select(e => e.Description)), []);
	}

	#endregion

	#region Roles

	public async Task<List<string>> GetUserRoles(string userId)
	{
		if (string.IsNullOrEmpty(userId))
			throw new ArgumentNullException(nameof(userId));

		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
			throw new NotFoundException($"User {userId} not found");

		var roles = await _userManager.GetRolesAsync(user);
		return [.. roles];
	}

	public async Task AddToRole(AddToRoleReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		var user = await _userManager.FindByIdAsync(request.UserId);
		if (user == null)
			throw new NotFoundException($"User {request.UserId} not found");

		var result = await _userManager.AddToRoleAsync(user, request.RoleName);
		if (!result.Succeeded)
			throw new ValidationException(string.Join("; ", result.Errors.Select(e => e.Description)), []);
	}

	public async Task RemoveFromRole(RemoveFromRoleReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		var user = await _userManager.FindByIdAsync(request.UserId);
		if (user == null)
			throw new NotFoundException($"User {request.UserId} not found");

		var result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);
		if (!result.Succeeded)
			throw new ValidationException(string.Join("; ", result.Errors.Select(e => e.Description)), []);
	}

	#endregion

	#region Claims

	public async Task<List<UserClaimDto>> GetUserClaims(string userId)
	{
		if (string.IsNullOrEmpty(userId))
			throw new ArgumentNullException(nameof(userId));

		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
			throw new NotFoundException($"User {userId} not found");

		var claims = await _userManager.GetClaimsAsync(user);
		return claims.Select(c => new UserClaimDto { Type = c.Type, Value = c.Value }).ToList();
	}

	public async Task AddUserClaim(AddUserClaimReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		var user = await _userManager.FindByIdAsync(request.UserId);
		if (user == null)
			throw new NotFoundException($"User {request.UserId} not found");

		var result = await _userManager.AddClaimAsync(user, new Claim(request.ClaimType, request.ClaimValue));
		if (!result.Succeeded)
			throw new ValidationException(string.Join("; ", result.Errors.Select(e => e.Description)), []);
	}

	public async Task RemoveUserClaim(RemoveUserClaimReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		var user = await _userManager.FindByIdAsync(request.UserId);
		if (user == null)
			throw new NotFoundException($"User {request.UserId} not found");

		var result = await _userManager.RemoveClaimAsync(user, new Claim(request.ClaimType, request.ClaimValue));
		if (!result.Succeeded)
			throw new ValidationException(string.Join("; ", result.Errors.Select(e => e.Description)), []);
	}

	#endregion

	#region Read

	public async Task<UserDto> GetUserById(string userId)
	{
		if (string.IsNullOrEmpty(userId))
			throw new ArgumentNullException(nameof(userId));

		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
			throw new NotFoundException($"User {userId} not found");

		return MapToDto(user);
	}

	public async Task<UserDto> GetUserByEmail(string email)
	{
		if (string.IsNullOrEmpty(email))
			throw new ArgumentNullException(nameof(email));

		var user = await _userManager.FindByEmailAsync(email);
		if (user == null)
			throw new NotFoundException($"User with email {email} not found");

		return MapToDto(user);
	}

	public async Task<List<UserDto>> GetAllUsers()
	{
		var users = await _userManager.Users.AsNoTracking().ToListAsync();
		return users.Select(MapToDto).ToList();
	}

	#endregion

	#region Private Helpers

	private static UserDto MapToDto(IdentityUser user)
	{
		return new UserDto
		{
			Id = user.Id,
			UserName = user.UserName ?? string.Empty,
			Email = user.Email ?? string.Empty,
			PhoneNumber = user.PhoneNumber ?? string.Empty,
			EmailConfirmed = user.EmailConfirmed,
			LockoutEnabled = user.LockoutEnabled,
			LockoutEnd = user.LockoutEnd
		};
	}

	#endregion
}
