using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.DTOs.v1;
using Dyvenix.App1.Auth.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.ApiClients;

namespace Dyvenix.App1.Auth.Shared.ApiClients.v1;

public class UserApiClient : ApiClientBase, IUserService
{
	public UserApiClient(HttpClient httpClient) : base(httpClient)
	{
	}

	#region Create

	public async Task<UserDto> CreateUser(CreateUserReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		return await PostAsync<UserDto>("api/Auth/v1/User/CreateUser", request);
	}

	#endregion

	#region Delete

	public async Task DeleteUser(string userId)
	{
		if (string.IsNullOrEmpty(userId))
			throw new ArgumentNullException(nameof(userId));

		await DeleteAsync<bool>($"api/Auth/v1/User/DeleteUser/{userId}", new { });
	}

	#endregion

	#region Update

	public async Task UpdateUser(UpdateUserReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		await PutAsync("api/Auth/v1/User/UpdateUser", request);
	}

	#endregion

	#region Roles

	public async Task<List<string>> GetUserRoles(string userId)
	{
		return await GetAsync<List<string>>($"api/Auth/v1/User/GetUserRoles/{userId}");
	}

	public async Task AddToRole(AddToRoleReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		await PostAsync("api/Auth/v1/User/AddToRole", request);
	}

	public async Task RemoveFromRole(RemoveFromRoleReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		await PostAsync("api/Auth/v1/User/RemoveFromRole", request);
	}

	#endregion

	#region Claims

	public async Task<List<UserClaimDto>> GetUserClaims(string userId)
	{
		return await GetAsync<List<UserClaimDto>>($"api/Auth/v1/User/GetUserClaims/{userId}");
	}

	public async Task AddUserClaim(AddUserClaimReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		await PostAsync("api/Auth/v1/User/AddUserClaim", request);
	}

	public async Task RemoveUserClaim(RemoveUserClaimReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		await PostAsync("api/Auth/v1/User/RemoveUserClaim", request);
	}

	#endregion

	#region Read

	public async Task<UserDto> GetUserById(string userId)
	{
		return await GetAsync<UserDto>($"api/Auth/v1/User/GetUserById/{userId}");
	}

	public async Task<UserDto> GetUserByEmail(string email)
	{
		return await GetAsync<UserDto>($"api/Auth/v1/User/GetUserByEmail/{email}");
	}

	public async Task<List<UserDto>> GetAllUsers()
	{
		return await GetAsync<List<UserDto>>("api/Auth/v1/User/GetAllUsers");
	}

	#endregion
}
