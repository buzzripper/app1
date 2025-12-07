namespace Dyvenix.App1.Functions.DTOs.EntraId;

public class AuthenticationContext
{
    public string CorrelationId { get; set; } = string.Empty;
    public ClientInfo? Client { get; set; }
    public string Protocol { get; set; } = string.Empty;
    public ServicePrincipalInfo? ClientServicePrincipal { get; set; }
    public ServicePrincipalInfo? ResourceServicePrincipal { get; set; }
    public UserInfo? User { get; set; }
}
