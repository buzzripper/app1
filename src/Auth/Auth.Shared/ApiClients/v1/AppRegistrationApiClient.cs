using Dyvenix.App1.Common.Shared.ApiClients;
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.DTOs;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Shared.ApiClients.v1;

public partial class AppRegistrationApiClient : ApiClientBase, IAppRegistrationService
{
    public AppRegistrationApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<AppRegistrationDto?> GetById(string id)
        => await GetAsync<AppRegistrationDto?>($"api/auth/v1/appregistration/GetById/{id}");

    public async Task<AppRegistrationDto?> GetByClientId(string clientId)
        => await GetAsync<AppRegistrationDto?>($"api/auth/v1/appregistration/GetByClientId/{Uri.EscapeDataString(clientId)}");

    public async Task<IReadOnlyList<AppRegistrationDto>> GetAll()
        => await GetAsync<IReadOnlyList<AppRegistrationDto>>("api/auth/v1/appregistration/GetAll");

    public async Task<string> Create(CreateAppRegistrationReq request)
        => await PostAsync<string>("api/auth/v1/appregistration/Create", request);

    public async Task Update(UpdateAppRegistrationReq request)
        => await PutAsync("api/auth/v1/appregistration/Update", request);

    public async Task Delete(string id)
        => await DeleteAsync<bool>("api/auth/v1/appregistration/Delete", new { Id = id });
}
