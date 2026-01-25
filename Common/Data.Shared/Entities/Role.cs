namespace Dyvenix.App1.Data.Shared.Entities;

/// <summary>
/// Represents a role that groups permissions together for easier assignment to users.
/// Roles are tenant-specific.
/// </summary>
public class Role : BaseEntity
{
    /// <summary>Foreign key to the owning tenant.</summary>
    public Guid TenantId { get; set; }

    /// <summary>Navigation property to the owning tenant.</summary>
    public Tenant Tenant { get; set; } = null!;

    /// <summary>Unique name of the role within the tenant (e.g., "Administrator", "Manager", "Viewer").</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Description of the role and its purpose.</summary>
    public string? Description { get; set; }

    /// <summary>Indicates if this is a system-defined role that cannot be modified or deleted.</summary>
    public bool IsSystemRole { get; set; }

    /// <summary>Indicates if this is the default role assigned to new users.</summary>
    public bool IsDefault { get; set; }

    /// <summary>Indicates if this role is currently active and can be assigned.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Sort order for display purposes.</summary>
    public int SortOrder { get; set; }

    // Navigation properties

    /// <summary>Permissions included in this role.</summary>
    public ICollection<Permission> Permissions { get; set; } = [];

    /// <summary>Users assigned to this role.</summary>
    public ICollection<AppUser> Users { get; set; } = [];
}
