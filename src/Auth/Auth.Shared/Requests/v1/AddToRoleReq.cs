namespace Dyvenix.App1.Auth.Shared.Requests.v1;

public class AddToRoleReq
{
	public string UserId { get; set; } = null!;
	public string RoleName { get; set; } = null!;
}
