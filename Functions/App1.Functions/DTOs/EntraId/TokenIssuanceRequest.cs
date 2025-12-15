namespace App1.App1.Functions.DTOs.EntraId;

public class TokenIssuanceRequest
{
    public string Type { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public TokenIssuanceData? Data { get; set; }
}
