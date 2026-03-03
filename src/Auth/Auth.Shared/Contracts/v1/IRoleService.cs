using Dyvenix.App1.Auth.Shared.DTOs.v1;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Shared.Contracts.v1;

public interface IRoleService
{
	// Create
	Task<RoleDto> CreateRole(CreateRoleReq request);

	// Delete
	Task DeleteRole(string roleId);

	// Update
	Task UpdateRole(UpdateRoleReq request);

	// Read
	Task<RoleDto> GetRoleById(string roleId);
	Task<RoleDto> GetRoleByName(string roleName);
	Task<List<RoleDto>> GetAllRoles();
}
