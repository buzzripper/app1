namespace App1.Data.Shared.Entities;

/// <summary>
/// Assigns a role to a user within a specific organization membership.
/// </summary>
public class UserRole : AuditableEntityBase
{
	public Guid RoleId { get; set; }
	public Role Role { get; set; } = null!;
}
