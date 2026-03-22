using Dyvenix.App1.Common.Shared.ApiClients;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.DTOs;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Shared.ApiClients.v1;

public partial class TenantApiClient : ApiClientBase, ITenantService
{
	public TenantApiClient(HttpClient httpClient) : base(httpClient)
	{
	}

	public async Task<TenantDto?> GetById(Guid id)
	{
		return await GetAsync<TenantDto?>($"api/auth/v1/tenant/GetById/{id}");
	}

	public async Task<TenantDto?> GetBySlug(string slug)
	{
		return await GetAsync<TenantDto?>($"api/auth/v1/tenant/GetBySlug/{Uri.EscapeDataString(slug)}");
	}

	public async Task<IReadOnlyList<TenantDto>> GetAll()
	{
		return await GetAsync<IReadOnlyList<TenantDto>>("api/auth/v1/tenant/GetAll");
	}

	public async Task<Guid> Create(CreateTenantReq request)
	{
		return await PostAsync<Guid>("api/auth/v1/tenant/Create", request);
	}

	public async Task Update(UpdateTenantReq request)
	{
		await PutAsync("api/auth/v1/tenant/Update", request);
	}

	public async Task Delete(Guid id)
	{
		var deleteReq = new DeleteReq { Id = id };
		await DeleteAsync<bool>("api/auth/v1/tenant/Delete", deleteReq);
	}
}
