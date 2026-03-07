using Dyvenix.App1.Auth.Shared.DTOs;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Shared.Contracts.v1;

public interface IRoleClaimService
{
    Task<RoleClaimDto?> GetById(int id);
    Task<IReadOnlyList<RoleClaimDto>> GetAllByRole(string roleId);
    Task Create(CreateRoleClaimReq request);
    Task Update(UpdateRoleClaimReq request);
    Task Delete(int id);
}
