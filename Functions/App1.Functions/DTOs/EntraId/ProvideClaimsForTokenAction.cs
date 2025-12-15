using System.Text.Json.Serialization;

namespace App1.App1.Functions.DTOs.EntraId;

public sealed class ProvideClaimsForTokenAction
{
    [JsonPropertyName("@odata.type")]
    public string ODataType { get; set; } = "microsoft.graph.tokenIssuanceStart.provideClaimsForToken";

    // Values can be string or array; using object for flexibility
    [JsonPropertyName("claims")]
    public Dictionary<string, object> Claims { get; set; } = new();
}
