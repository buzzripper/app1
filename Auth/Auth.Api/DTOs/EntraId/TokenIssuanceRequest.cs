namespace Dyvenix.Auth.Api.DTOs.EntraId;

public class TokenIssuanceRequest
{
	public string Type { get; set; }
	public string Source { get; set; }
	public TokenIssuanceData Data { get; set; }
}