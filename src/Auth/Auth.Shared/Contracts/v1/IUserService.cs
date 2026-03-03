using Dyvenix.App1.Auth.Shared.DTOs.v1;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Shared.Contracts.v1;

public interface IUserService
{
	// Create
	Task<UserDto> CreateUser(CreateUserReq request);

	// Delete
	Task DeleteUser(string userId);

	// Update
	Task UpdateUser(UpdateUserReq request);

	// Roles
	Task<List<string>> GetUserRoles(string userId);
	Task AddToRole(AddToRoleReq request);
	Task RemoveFromRole(RemoveFromRoleReq request);

	// Claims
	Task<List<UserClaimDto>> GetUserClaims(string userId);
	Task AddUserClaim(AddUserClaimReq request);
	Task RemoveUserClaim(RemoveUserClaimReq request);

	// Read
	Task<UserDto> GetUserById(string userId);
	Task<UserDto> GetUserByEmail(string email);
	Task<List<UserDto>> GetAllUsers();
}
