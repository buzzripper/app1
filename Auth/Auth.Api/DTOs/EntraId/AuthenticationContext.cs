namespace Dyvenix.Auth.Api.DTOs.EntraId;

public class AuthenticationContext
{
	public string CorrelationId { get; set; }
	public ClientInfo Client { get; set; }
	public string Protocol { get; set; }
	public ServicePrincipalInfo ClientServicePrincipal { get; set; }
	public ServicePrincipalInfo ResourceServicePrincipal { get; set; }
	public UserInfo User { get; set; }
}