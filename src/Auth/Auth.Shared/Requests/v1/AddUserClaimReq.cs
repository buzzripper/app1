namespace Dyvenix.App1.Auth.Shared.Requests.v1;

public class AddUserClaimReq
{
	public string UserId { get; set; } = null!;
	public string ClaimType { get; set; } = null!;
	public string ClaimValue { get; set; } = null!;
}
