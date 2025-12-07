using System.Text.Json.Serialization;

namespace Dyvenix.App1.Functions.DTOs.EntraId;

public sealed class TokenIssuanceResponseData
{
    [JsonPropertyName("@odata.type")]
    public string ODataType { get; set; } = "microsoft.graph.onTokenIssuanceStartResponseData";

    [JsonPropertyName("actions")]
    public List<ProvideClaimsForTokenAction> Actions { get; set; } = new();
}
