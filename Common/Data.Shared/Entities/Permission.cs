namespace Dyvenix.App1.Data.Shared.Entities;

/// <summary>
/// Represents an atomic permission that can be assigned to roles or directly to users.
/// Permissions are global (not tenant-specific) and defined at the system level.
/// </summary>
public class Permission : BaseEntity
{
    /// <summary>
    /// Unique permission key/value (e.g., "users.read", "orders.create", "reports.export").
    /// This is the atomic permission string used in authorization checks.
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>Human-readable display name for the permission.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Description of what this permission grants.</summary>
    public string? Description { get; set; }

    /// <summary>Category/group for organizing permissions in the UI (e.g., "Users", "Orders", "Reports").</summary>
    public string? Category { get; set; }

    /// <summary>Sort order for display purposes within a category.</summary>
    public int SortOrder { get; set; }

    /// <summary>Indicates if this permission is currently active and can be assigned.</summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties

    /// <summary>Roles that have this permission.</summary>
    public ICollection<Role> Roles { get; set; } = [];

    /// <summary>Users directly assigned this permission (bypassing role membership).</summary>
    public ICollection<AppUser> Users { get; set; } = [];
}
