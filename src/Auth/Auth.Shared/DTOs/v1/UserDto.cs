namespace Dyvenix.App1.Auth.Shared.DTOs.v1;

public class UserDto
{
	public string Id { get; set; } = null!;
	public string UserName { get; set; } = null!;
	public string Email { get; set; } = null!;
	public string PhoneNumber { get; set; } = null!;
	public bool EmailConfirmed { get; set; }
	public bool LockoutEnabled { get; set; }
	public DateTimeOffset? LockoutEnd { get; set; }
}
