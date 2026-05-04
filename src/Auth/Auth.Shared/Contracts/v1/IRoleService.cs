using Dyvenix.App1.Auth.Shared.DTOs;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Shared.Contracts.v1;

public interface IRoleService
{
    Task<RoleDto?> GetById(string id);
    Task<RoleDto?> GetByName(string name);
    Task<IReadOnlyList<RoleDto>> GetAllByTenant(Guid tenantId);
    Task<string> Create(CreateRoleReq request);
    Task Update(UpdateRoleReq request);
    Task Delete(string id);
}
