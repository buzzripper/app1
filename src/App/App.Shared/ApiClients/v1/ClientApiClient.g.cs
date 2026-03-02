//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/1/2026 10:25 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Common.Shared.ApiClients;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Requests.v1;
using Dyvenix.App1.App.Shared.Dtos;

namespace Dyvenix.App1.App.Shared.ApiClients.v1;

public partial class ClientApiClient : ApiClientBase, IClientService
{
	public ClientApiClient(HttpClient httpClient) : base(httpClient)
	{
	}
	
	#region Delete
	
	public async Task Delete(Guid id)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id));
	
		var deleteReq = new DeleteReq { Id = id };	
		await DeleteAsync<bool>($"api/App/v1/Client/DeleteClient", deleteReq);
	}
	
	#endregion

	#region Updates
	
	public async Task<byte[]> Create(CreateReq request)
	{
		return await PatchAsync<byte[]>($"api/App/v1/Client/Create", request);
	}

	#endregion

	#region Read Methods - Single
	
	public async Task<ClientDto> GetById(Guid id)
	{
		return await GetAsync<ClientDto>($"api/App/v1/Client/GetById/{id}");
	}
	
	public async Task<ClientDto> GetByKey(string key)
	{
		return await GetAsync<ClientDto>($"api/App/v1/Client/GetByKey/{key}");
	}

	#endregion

	#region Read Methods - List
	
	public async Task<IReadOnlyList<ClientOptionDto>> GetAllClientOptions(GetAllClientOptionsReq request)
	{
		return await PostAsync<IReadOnlyList<ClientOptionDto>>($"api/App/v1/Client/GetAllClientOptions", request);
	}
	
	public async Task<IReadOnlyList<ClientRouteDto>> GetAllRoutes()
	{
		return await GetAsync<IReadOnlyList<ClientRouteDto>>($"api/App/v1/Client/GetAllRoutes");
	}

	#endregion
}
