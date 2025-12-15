using System.Text.Json.Serialization;

namespace App1.Auth.Api.DTOs.EntraId;

public class TokenIssuanceData
{
	[JsonPropertyName("@odata.type")]
	public string ODataType { get; set; }

	public string TenantId { get; set; }
	public string AuthenticationEventListenerId { get; set; }
	public string CustomAuthenticationExtensionId { get; set; }
	public AuthenticationContext AuthenticationContext { get; set; }
}