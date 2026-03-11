//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/10/2026 9:58:05 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Common.Shared.ApiClients;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.DTOs;
using Dyvenix.App1.App.Shared.Dtos;

namespace Dyvenix.App1.App.Shared.ApiClients.v1;

public partial class ClientApiClient : ApiClientBase, IClientService
{
	public ClientApiClient(HttpClient httpClient) : base(httpClient)
	{
	}
	
	#region Delete
	
	public async Task DeleteClient(Guid id)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id));
	
		var deleteReq = new DeleteReq { Id = id };	
		await DeleteAsync<bool>($"api/App/v1/Client/DeleteClient", deleteReq);
	}
	
	#endregion

	#region Updates
	
	public async Task<byte[]> CreateClient(CreateClientReq request)
	{
		return await PatchAsync<byte[]>($"api/App/v1/Client/CreateClient", request);
	}
	
	public async Task<byte[]> UpdateClient(UpdateClientReq request)
	{
		return await PatchAsync<byte[]>($"api/App/v1/Client/UpdateClient", request);
	}
	
	public async Task<byte[]> UpdateClientBaseUrl(UpdateClientBaseUrlReq request)
	{
		return await PatchAsync<byte[]>($"api/App/v1/Client/UpdateClientBaseUrl", request);
	}

	#endregion

	#region Read Methods - Single
	
	public async Task<ClientDto> GetClientById(Guid id)
	{
		return await GetAsync<ClientDto>($"api/App/v1/Client/GetClientById/{id}");
	}
	
	public async Task<ClientDto> GetClientByKey(string key)
	{
		return await GetAsync<ClientDto>($"api/App/v1/Client/GetClientByKey/{key}");
	}

	#endregion

	#region Read Methods - List
	
	public async Task<IReadOnlyList<ClientLookupDto>> GetAllClientLookupItems(GetAllClientLookupItemsReq request)
	{
		return await PostAsync<IReadOnlyList<ClientLookupDto>>($"api/App/v1/Client/GetAllClientLookupItems", request);
	}
	
	public async Task<IReadOnlyList<ClientRouteDto>> GetAllClientRoutes()
	{
		return await GetAsync<IReadOnlyList<ClientRouteDto>>($"api/App/v1/Client/GetAllClientRoutes");
	}
	
	public async Task<IReadOnlyList<ClientDto>> GetAllClients(GetAllClientsReq request)
	{
		return await PostAsync<IReadOnlyList<ClientDto>>($"api/App/v1/Client/GetAllClients", request);
	}
	
	public async Task<ListPage<ClientLookupDto>> SearchClientsByName(SearchClientsByNameReq request)
	{
		return await PostAsync<ListPage<ClientLookupDto>>($"api/App/v1/Client/SearchClientsByName", request);
	}

	#endregion
}
