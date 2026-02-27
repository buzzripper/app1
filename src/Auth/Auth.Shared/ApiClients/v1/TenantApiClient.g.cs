//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/27/2026 4:53 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Common.Shared.ApiClients;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.Requests.v1;
using Dyvenix.App1.Auth.Shared.DTOs;

namespace Dyvenix.App1.Auth.Shared.ApiClients.v1;

public partial class TenantApiClient : ApiClientBase, ITenantService
{
	public TenantApiClient(HttpClient httpClient) : base(httpClient)
	{
	}
	
	#region Delete
	
	public async Task DeleteTenant(Guid id)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id));
	
		var deleteReq = new DeleteReq { Id = id };	
		await DeleteAsync<bool>($"api/Auth/v1/Tenant/DeleteTenant", deleteReq);
	}
	
	#endregion

	#region Updates
	
	public async Task UpdateName(UpdateNameReq request)
	{
		await PatchAsync($"api/Auth/v1/Tenant/UpdateName", request);
	}

	#endregion

	#region Read Methods - Single
	
	public async Task<Dto2> GetById(Guid id)
	{
		return await GetAsync<Dto2>($"api/Auth/v1/Tenant/GetById/{id}");
	}

	#endregion

	#region Read Methods - List
	
	public async Task<IReadOnlyList<Dto1>> GetAll()
	{
		return await GetAsync<IReadOnlyList<Dto1>>($"api/Auth/v1/Tenant/GetAll");
	}

	#endregion
}
