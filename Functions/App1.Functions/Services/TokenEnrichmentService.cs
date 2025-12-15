using App1.App1.Functions.DTOs.EntraId;

namespace App1.App1.Functions.Services;

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
                ["perm"] = new[] { "app1.admin", "auth.none", "portal.read" }
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
