using Dyvenix.App1.Functions.DTOs.EntraId;

namespace Dyvenix.App1.Functions.Services;

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
                ["DateOfBirth"] = "12/23/1964",
                ["permissions"] = new[] { "ar_read", "ar_write", "ap_read" },
                ["perm"] = "app1.admin",
                ["perm"] = "auth.none",
                ["perm"] = "foo.bar",
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
