namespace App1.Data.Shared.Entities;

public class Role
{
	public Guid OrganizationId { get; set; }
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;

	public string NormalizedName { get; set; } = string.Empty;
	public string? Description { get; set; }

	public bool IsSystemRole { get; set; } // e.g., "Owner", "Admin" shipped by you

	//public ICollection<RolePermission> Permissions { get; set; } = new List<RolePermission>();
	public ICollection<UserRole> Members { get; set; } = new List<UserRole>();
}
