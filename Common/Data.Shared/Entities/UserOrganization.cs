namespace Dyvenix.App1.Data.Shared.Entities;

/// <summary>
/// Represents a user's membership in an organization.
/// Users can be members of multiple organizations within their tenant.
/// </summary>
public class UserOrganization : BaseEntity
{
    /// <summary>Foreign key to the user.</summary>
    public Guid UserId { get; set; }

    /// <summary>Navigation property to the user.</summary>
    public AppUser User { get; set; } = null!;

    /// <summary>Foreign key to the organization.</summary>
    public Guid OrganizationId { get; set; }

    /// <summary>Navigation property to the organization.</summary>
    public Organization Organization { get; set; } = null!;

    /// <summary>Indicates if this is the user's primary organization membership.</summary>
    public bool IsPrimary { get; set; }

    /// <summary>UTC timestamp when the user joined this organization.</summary>
    public DateTime JoinedAt { get; set; }

    /// <summary>UTC timestamp when the membership ends (null for indefinite).</summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>Optional notes about this membership.</summary>
    public string? Notes { get; set; }
}
