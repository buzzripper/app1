namespace Dyvenix.App1.Data.Shared.Entities;

/// <summary>
/// Represents an organizational unit within a tenant's hierarchy.
/// Supports n-level hierarchy via self-referential ParentId with an OrgLevel indicator.
/// </summary>
public class Organization : BaseEntity
{
    /// <summary>Foreign key to the owning tenant.</summary>
    public Guid TenantId { get; set; }

    /// <summary>Navigation property to the owning tenant.</summary>
    public Tenant Tenant { get; set; } = null!;

    /// <summary>Display name of the organization unit.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Short code/identifier for the organization (e.g., "NA-EAST-001").</summary>
    public string? Code { get; set; }

    /// <summary>Description of the organization unit.</summary>
    public string? Description { get; set; }

    /// <summary>Hierarchical level indicator for easier querying.</summary>
    public OrgLevel Level { get; set; } = OrgLevel.Office;

    /// <summary>Foreign key to the parent organization (null for root/top-level).</summary>
    public Guid? ParentId { get; set; }

    /// <summary>Navigation property to the parent organization.</summary>
    public Organization? Parent { get; set; }

    /// <summary>Child organizations in the hierarchy.</summary>
    public ICollection<Organization> Children { get; set; } = [];

    /// <summary>Indicates if this organization is currently active.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Sort order for display purposes among siblings.</summary>
    public int SortOrder { get; set; }

    /// <summary>Primary email contact for this organization.</summary>
    public string? Email { get; set; }

    /// <summary>Primary phone number for this organization.</summary>
    public string? Phone { get; set; }

    /// <summary>Street address line 1.</summary>
    public string? AddressLine1 { get; set; }

    /// <summary>Street address line 2.</summary>
    public string? AddressLine2 { get; set; }

    /// <summary>City.</summary>
    public string? City { get; set; }

    /// <summary>State or province.</summary>
    public string? StateProvince { get; set; }

    /// <summary>Postal/ZIP code.</summary>
    public string? PostalCode { get; set; }

    /// <summary>Country code (ISO 3166-1 alpha-2).</summary>
    public string? CountryCode { get; set; }

    /// <summary>IANA timezone identifier for this organization's local timezone.</summary>
    public string? TimeZone { get; set; }

    /// <summary>Organization-specific settings stored as JSON.</summary>
    public string? SettingsJson { get; set; }

    // Navigation properties

    /// <summary>User memberships in this organization.</summary>
    public ICollection<UserOrganization> UserOrganizations { get; set; } = [];
}
