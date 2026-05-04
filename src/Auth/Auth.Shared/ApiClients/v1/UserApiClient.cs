using Dyvenix.App1.Common.Shared.ApiClients;
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.DTOs;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Shared.ApiClients.v1;

public partial class UserApiClient : ApiClientBase, IUserService
{
    public UserApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<UserDto?> GetById(string id)
        => await GetAsync<UserDto?>($"api/auth/v1/user/GetById/{id}");

    public async Task<UserDto?> GetByEmail(string email)
        => await GetAsync<UserDto?>($"api/auth/v1/user/GetByEmail/{Uri.EscapeDataString(email)}");

    public async Task<IReadOnlyList<UserSummaryDto>> GetAllByTenant(Guid tenantId)
        => await GetAsync<IReadOnlyList<UserSummaryDto>>($"api/auth/v1/user/GetAllByTenant/{tenantId}");

    public async Task<string> Create(CreateUserReq request)
        => await PostAsync<string>("api/auth/v1/user/Create", request);

    public async Task Update(UpdateUserReq request)
        => await PutAsync<bool>("api/auth/v1/user/Update", request);

    public async Task Delete(string id)
        => await DeleteAsync<bool>("api/auth/v1/user/Delete", new { Id = id });
}
