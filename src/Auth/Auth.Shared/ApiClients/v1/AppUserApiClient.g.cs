//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/12/2026 8:04 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Common.Shared.ApiClients;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Shared.ApiClients.v1;

public interface IAppUserApiClient
{
	Task<Guid> CreateAppUser(AppUser appUser);
	Task<bool> DeleteAppUser(Guid id);
	Task<byte[]> UpdateAppUser(AppUser appUser);
	Task<byte[]> UpdateUsername(UpdateUsernameReq request);
	Task<AppUser> GetById(Guid id);
	Task<List<AppUser>> ReqByUsername(string username);
}

public partial class AppUserApiClient : ApiClientBase, IAppUserApiClient
{
	public AppUserApiClient(HttpClient httpClient) : base(httpClient)
	{
	}
	
	#region Create
	
	public async Task<Guid> CreateAppUser(AppUser appUser)
	{
		ArgumentNullException.ThrowIfNull(appUser);
	
		return await PostAsync<Guid>("api/v1/AppUser/CreateAppUser", appUser);
	}
	
	#endregion
	
	#region Delete
	
	public async Task<bool> DeleteAppUser(Guid id)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id));
	
		var deleteReq = new DeleteReq { Id = id };	
		return await DeleteAsync<bool>($"api/v1/AppUser/DeleteAppUser", deleteReq);
	}
	
	#endregion

	#region Updates
	
	public async Task<byte[]> UpdateAppUser(AppUser appUser)
	{
		ArgumentNullException.ThrowIfNull(appUser);
		return await PutAsync<byte[]>("api/v1/AppUser/UpdateAppUser", appUser);
	}
	
	public async Task<byte[]> UpdateUsername(UpdateUsernameReq request)
	{
		return await PatchAsync<byte[]>($"api/v1/AppUser/UpdateUsername", request);
	}

	#endregion

	#region Read Methods - Single
	
	public async Task<AppUser> GetById(Guid id)
	{
		return await GetAsync<AppUser>($"api/v1/AppUser/GetById/{id}");
	}

	#endregion

	#region Read Methods - List
	
	public async Task<List<AppUser>> ReqByUsername(string username)
	{
		return await GetAsync<List<AppUser>>($"api/v1/AppUser/ReqByUsername/{username}");
	}

	#endregion
}
