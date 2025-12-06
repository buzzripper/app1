using System.Text.Json.Serialization;

namespace Dyvenix.Auth.Api.DTOs.EntraId;

public sealed class TokenIssuanceResponse
{
	[JsonPropertyName("data")]
	public TokenIssuanceResponseData Data { get; set; } = new();
}