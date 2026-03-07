using Dyvenix.App1.AdAgent.Shared.Contracts.v1;
using Dyvenix.App1.AdAgent.Shared.DTOs;
using Dyvenix.App1.AdAgent.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.ApiClients;

namespace Dyvenix.App1.AdAgent.Shared.ApiClients.v1;

public partial class AdApiClient : ApiClientBase, IAdService
{
	public AdApiClient(HttpClient httpClient) : base(httpClient)
	{
	}

	public async Task<AdAuthResult> AuthenticateUser(string userUpnOrDomainUser, string password, CancellationToken ct = default)
	{
		var request = new AuthenticateUserReq
		{
			UserUpnOrDomainUser = userUpnOrDomainUser,
			Password = password
		};
		return await PostAsync<AdAuthResult>($"api/adagent/v1/ad/AuthenticateUser", request);
	}
}

