//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/7/2026 7:55 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.Auth.Shared.DTOs;

namespace Dyvenix.App1.Auth.Api.Auth.Shared.ApiClients.v1;

public interface IAppUserApiClient
{
Task<Guid> CreateAppUser(AppUser appUser)
Task<bool> DeleteAppUser(Guid id)
Task<byte[]> UpdateAppUser(AppUser appUser)
Task<byte[]> UpdateUsername(UpdateUsernameReq request)
Task<AppUser> GetById(Guid id)
Task<List<AppUser>> ReqByUsername(string username)
}

public partial class AppUserApiClient : ApiClientBase<AppUser>, IAppUserApiClient
{
		public class AppUserApiClient(HttpClient httpClient) : base(httpClient)
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
			return await PostAsync<bool>($"api/v1/AppUser/DeleteAppUser/{id}", null);
		}
	
		#endregion

	#region Updates
	
		#region Update
	
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

	#endregion

	#region Read Methods - Single
		#region Single Methods
	
		public async Task<AppUser> GetById(Guid id)
		{
			return await GetAsync<AppUser>($"api/v1/AppUser/GetById/{id}");
		}
	
		#endregion

	#endregion

	#region Read Methods - List
		#region List Methods
	
		public async Task<List<AppUser>> ReqByUsername(string username)
		{
			return await GetAsync<List<AppUser>>($"api/v1/AppUser/ReqByUsername/{username}");
		}
	
		#endregion

	#endregion
}
