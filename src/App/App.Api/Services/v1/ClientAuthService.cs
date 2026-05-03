using Dyvenix.App1.App.Api.Contracts.v1;
using Dyvenix.App1.Auth.Shared.ApiClients.v1;
using Dyvenix.App1.Auth.Shared.DTOs;
using Dyvenix.App1.Auth.Shared.Requests.v1;
using Microsoft.Extensions.Logging;

namespace Dyvenix.App1.App.Api.Services.v1;

public class ClientAuthService(TenantApiClient tenantApiClient, ILogger<ClientAuthService> logger) : IClientAuthService
{
	public async Task<TenantDto?> GetTenantById(Guid id)
	{
		return await tenantApiClient.GetById(id);
	}

	public async Task<TenantDto?> GetTenantBySlug(string slug)
	{
		return await tenantApiClient.GetBySlug(slug);
	}

	public async Task<IReadOnlyList<TenantDto>> GetAllTenants()
	{
		return await tenantApiClient.GetAll();
	}

	public async Task<Guid> CreateTenant(CreateTenantReq request)
	{
		return await tenantApiClient.Create(request);
	}

	public async Task UpdateTenant(UpdateTenantReq request)
	{
		await tenantApiClient.Update(request);
	}

	public async Task DeleteTenant(Guid id)
	{
		await tenantApiClient.Delete(id);
	}
}
