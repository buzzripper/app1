using Dyvenix.App1.Common.Shared.ApiClients;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.DTOs;

namespace Dyvenix.App1.Auth.Shared.ApiClients.v1;

public partial class TenantApiClient : ApiClientBase, ITenantService
{
	public TenantApiClient(HttpClient httpClient) : base(httpClient)
	{
	}
}
