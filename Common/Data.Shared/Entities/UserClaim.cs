namespace App1.Data.Shared.Entities;

public class UserClaim
{
	public Guid UserId { get; set; }
	public string Type { get; set; } = string.Empty;
	public string Value { get; set; } = string.Empty;
}
