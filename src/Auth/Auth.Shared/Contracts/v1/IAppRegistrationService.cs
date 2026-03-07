using Dyvenix.App1.Auth.Shared.DTOs;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Shared.Contracts.v1;

public interface IAppRegistrationService
{
    Task<AppRegistrationDto?> GetById(string id);
    Task<AppRegistrationDto?> GetByClientId(string clientId);
    Task<IReadOnlyList<AppRegistrationDto>> GetAll();
    Task<string> Create(CreateAppRegistrationReq request);
    Task Update(UpdateAppRegistrationReq request);
    Task Delete(string id);
}
