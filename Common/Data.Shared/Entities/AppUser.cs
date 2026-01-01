namespace App1.Data.Shared.Entities;

/// <summary>
/// A person/account in your system. Authentication happens at an external IdP.
/// Authorization is enforced via roles/claims in your DB.
/// </summary>
public class AppUser : AuditableEntityBase
{
	public Guid OrganizationId { get; set; }

	/// <summary>
	/// IdP subject/oid/etc. (e.g. Entra "oid" or "sub"). Not unique globally unless combined with Provider.
	/// </summary>
	public string? ExternalId { get; set; }

	/// <summary>
	/// Identifier for the IdP (e.g. "EntraID", "EntraExternal", "Auth0", "Okta").
	/// </summary>
	public string? ExternalProvider { get; set; }

	public string? Email { get; set; }

	/// <summary>Store a normalized email for case-insensitive lookups.</summary>
	public string? NormalizedEmail { get; set; }

	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? DisplayName { get; set; }
	public string? PhoneNumber { get; set; }
	public bool EmailVerified { get; set; }
	public bool PhoneVerified { get; set; }

	public UserStatus Status { get; set; } = UserStatus.Active;

	/// <summary>
	/// Optional JSON metadata (preferences, UI settings, etc.).
	/// </summary>
	public string? MetadataJson { get; set; }

	public ICollection<UserClaim> Claims { get; set; } = null!;
	public Organization Organization { get; set; } = null!;
}
