using Dyvenix.App1.Common.Shared.ApiClients;
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.DTOs;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Shared.ApiClients.v1;

public partial class RoleApiClient : ApiClientBase, IRoleService
{
    public RoleApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<RoleDto?> GetById(string id)
        => await GetAsync<RoleDto?>($"api/auth/v1/role/GetById/{id}");

    public async Task<RoleDto?> GetByName(string name)
        => await GetAsync<RoleDto?>($"api/auth/v1/role/GetByName/{Uri.EscapeDataString(name)}");

    public async Task<IReadOnlyList<RoleDto>> GetAllByTenant(Guid tenantId)
        => await GetAsync<IReadOnlyList<RoleDto>>($"api/auth/v1/role/GetAllByTenant/{tenantId}");

    public async Task<string> Create(CreateRoleReq request)
        => await PostAsync<string>("api/auth/v1/role/Create", request);

    public async Task Update(UpdateRoleReq request)
        => await PutAsync("api/auth/v1/role/Update", request);

    public async Task Delete(string id)
        => await DeleteAsync<bool>("api/auth/v1/role/Delete", new { Id = id });
}
