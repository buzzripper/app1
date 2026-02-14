//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/14/2026 11:12 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Common.Shared.ApiClients;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Shared.ApiClients.v1;

public partial class AppUserApiClient : ApiClientBase, IAppUserService
{
	public AppUserApiClient(HttpClient httpClient) : base(httpClient)
	{
	}
	
	#region Create
	
	public async Task CreateAppUser(AppUser appUser)
	{
		ArgumentNullException.ThrowIfNull(appUser);
	
		await PostAsync("api/v1/AppUser/CreateAppUser", appUser);
	}
	
	#endregion
	
	#region Delete
	
	public async Task DeleteAppUser(Guid id)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id));
	
		var deleteReq = new DeleteReq { Id = id };	
		await DeleteAsync<bool>($"api/v1/AppUser/DeleteAppUser", deleteReq);
	}
	
	#endregion

	#region Updates
	
	public async Task UpdateAppUser(AppUser appUser)
	{
		ArgumentNullException.ThrowIfNull(appUser);
		await PutAsync("api/v1/AppUser/UpdateAppUser", appUser);
	}
	
	public async Task UpdateUsername(UpdateUsernameReq request)
	{
		await PatchAsync($"api/v1/AppUser/UpdateUsername", request);
	}

	#endregion

	#region Read Methods - Single
	
	public async Task<AppUser> GetById(Guid id)
	{
		return await GetAsync<AppUser>($"api/v1/AppUser/GetById/{id}");
	}

	#endregion

	#region Read Methods - List
	
	public async Task<List<AppUser>> ReqByUsername(ReqByUsernameReq request)
	{
		return await GetAsync<List<AppUser>>($"api/v1/AppUser/ReqByUsername");
	}

	#endregion
}
