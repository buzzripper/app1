using System.Text.Json.Serialization;

namespace Dyvenix.App1.Functions.DTOs.EntraId;

public sealed class TokenIssuanceResponse
{
    [JsonPropertyName("data")]
    public TokenIssuanceResponseData Data { get; set; } = new();
}
