namespace Dyvenix.App1.Auth.Shared.Requests.v1;

public class CreateUserReq
{
	public string UserName { get; set; } = null!;
	public string Email { get; set; } = null!;
	public string Password { get; set; } = null!;
	public string PhoneNumber { get; set; } = null!;
}
