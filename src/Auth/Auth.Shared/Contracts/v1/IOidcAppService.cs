using Dyvenix.App1.Auth.Shared.DTOs;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Shared.Contracts.v1;

public interface IOidcAppService
{
    Task<OidcAppDto?> GetById(string id);
    Task<OidcAppDto?> GetByClientId(string clientId);
    Task<IReadOnlyList<OidcAppDto>> GetAll();
    Task<string> Create(CreateOidcAppReq request);
    Task Update(UpdateOidcAppReq request);
    Task Delete(string id);
}
