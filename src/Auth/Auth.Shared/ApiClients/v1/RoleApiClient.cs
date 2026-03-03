using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.DTOs.v1;
using Dyvenix.App1.Auth.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.ApiClients;

namespace Dyvenix.App1.Auth.Shared.ApiClients.v1;

public class RoleApiClient : ApiClientBase, IRoleService
{
	public RoleApiClient(HttpClient httpClient) : base(httpClient)
	{
	}

	#region Create

	public async Task<RoleDto> CreateRole(CreateRoleReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		return await PostAsync<RoleDto>("api/Auth/v1/Role/CreateRole", request);
	}

	#endregion

	#region Delete

	public async Task DeleteRole(string roleId)
	{
		if (string.IsNullOrEmpty(roleId))
			throw new ArgumentNullException(nameof(roleId));

		await DeleteAsync<bool>($"api/Auth/v1/Role/DeleteRole/{roleId}", new { });
	}

	#endregion

	#region Update

	public async Task UpdateRole(UpdateRoleReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		await PutAsync("api/Auth/v1/Role/UpdateRole", request);
	}

	#endregion

	#region Read

	public async Task<RoleDto> GetRoleById(string roleId)
	{
		return await GetAsync<RoleDto>($"api/Auth/v1/Role/GetRoleById/{roleId}");
	}

	public async Task<RoleDto> GetRoleByName(string roleName)
	{
		return await GetAsync<RoleDto>($"api/Auth/v1/Role/GetRoleByName/{roleName}");
	}

	public async Task<List<RoleDto>> GetAllRoles()
	{
		return await GetAsync<List<RoleDto>>("api/Auth/v1/Role/GetAllRoles");
	}

	#endregion
}
