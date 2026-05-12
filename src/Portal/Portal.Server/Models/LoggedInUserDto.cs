namespace Dyvenix.App1.Portal.Server.Models
{
	public class LoggedInUserDto
	{
		public static readonly LoggedInUserDto Anonymous = new() { IsAuthenticated = false };

		public bool IsAuthenticated { get; set; }
		public string UserId { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;
		public Guid? TenantId { get; set; }
	}
}
