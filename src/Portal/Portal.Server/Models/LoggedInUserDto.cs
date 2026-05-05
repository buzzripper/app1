namespace App1.App1.Portal.Server.Models;

public class LoggedInUserDto
{
	public static readonly LoggedInUserDto Anonymous = new() { IsAuthenticated = false };

	public bool IsAuthenticated { get; set; }
	public string UserId { get; set; } = string.Empty;
	public string UserName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public Guid? TenantId { get; set; }
	public IReadOnlyList<string> Roles { get; set; } = [];
	public IReadOnlyList<string> Permissions { get; set; } = [];
}
