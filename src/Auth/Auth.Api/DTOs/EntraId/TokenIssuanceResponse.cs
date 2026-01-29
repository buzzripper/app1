using System.Text.Json.Serialization;

namespace App1.Auth.Api.DTOs.EntraId;

public sealed class TokenIssuanceResponse
{
	[JsonPropertyName("data")]
	public TokenIssuanceResponseData Data { get; set; } = new();
}