using System.Text.Json.Serialization;

namespace App1.App1.Functions.DTOs.EntraId;

public class TokenIssuanceData
{
    [JsonPropertyName("@odata.type")]
    public string ODataType { get; set; } = string.Empty;

    public string TenantId { get; set; } = string.Empty;
    public string AuthenticationEventListenerId { get; set; } = string.Empty;
    public string CustomAuthenticationExtensionId { get; set; } = string.Empty;
    public AuthenticationContext? AuthenticationContext { get; set; }
}
