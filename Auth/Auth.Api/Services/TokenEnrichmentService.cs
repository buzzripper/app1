using Dyvenix.Auth.Api.DTOs.EntraId;
using System.Threading.Tasks;

namespace Dyvenix.Auth.Api.Services;

public interface ITokenEnrichmentService
{
	Task<TokenIssuanceResponse> GetClaims(TokenIssuanceRequest req);
}

public class TokenEnrichmentService : ITokenEnrichmentService
{
	public async Task<TokenIssuanceResponse> GetClaims(TokenIssuanceRequest req)
	{
		// Build the "provide claims" action
		var provideClaimsForTokenAction = new ProvideClaimsForTokenAction
		{
			ODataType = "microsoft.graph.tokenIssuanceStart.provideClaimsForToken",
			Claims =
				{
                    // Example custom claims you want in the token
                    ["DateOfBirth"] = "12/23/1960",
					["permissions"] = new[] { "ar_read", "ar_write", "ap_read" },
					["app1.user.access"] = "user",
					["ApiVersion"] = "v1",
					["CorrelationId"] = "8FF630AC-5AEC-4145-B532-B6CC55CBFB43"
				}
		};

		return new TokenIssuanceResponse
		{
			Data = new TokenIssuanceResponseData
			{
				ODataType = "microsoft.graph.onTokenIssuanceStartResponseData",
				Actions = new() { provideClaimsForTokenAction }
			}
		};
	}
}
