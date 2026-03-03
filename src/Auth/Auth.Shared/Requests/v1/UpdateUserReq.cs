namespace Dyvenix.App1.Auth.Shared.Requests.v1;

public class UpdateUserReq
{
	public string Id { get; set; } = null!;
	public string UserName { get; set; } = null!;
	public string Email { get; set; } = null!;
	public string PhoneNumber { get; set; } = null!;
}
