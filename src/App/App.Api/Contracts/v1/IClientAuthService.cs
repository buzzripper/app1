using Dyvenix.App1.Auth.Shared.DTOs;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.App.Api.Contracts.v1;

public interface IClientAuthService
{
	Task<TenantDto?> GetTenantById(Guid id);
	Task<TenantDto?> GetTenantBySlug(string slug);
	Task<IReadOnlyList<TenantDto>> GetAllTenants();
	Task<Guid> CreateTenant(CreateTenantReq request);
	Task UpdateTenant(UpdateTenantReq request);
	Task DeleteTenant(Guid id);
}
