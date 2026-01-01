namespace App1.Data.Shared.Entities;

/// <summary>
/// Represents a tenant / organization in a multi-tenant SaaS.
/// </summary>
public class Organization : AuditableEntityBase
{
	public bool IsSystem { get; set; }

	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// URL-friendly unique key for the tenant (e.g., used in subdomain or path).
	/// </summary>
	public string Slug { get; set; } = string.Empty;

	/// <summary>
	/// Optional external billing/customer identifier (Stripe customer id, etc.).
	/// </summary>
	public string? BillingExternalId { get; set; }

	/// <summary>
	/// Tenant status flag (active/suspended etc.).
	/// </summary>
	public OrgStatus Status { get; set; }

	/// <summary>
	/// Default locale/culture for the tenant.
	/// </summary>
	public string? DefaultLocale { get; set; }

	/// <summary>
	/// Default time zone id (IANA or Windows id - pick one standard).
	/// </summary>
	public string? DefaultTimeZone { get; set; }

	/// <summary>
	/// Optional metadata as JSON (feature flags, settings, etc.).
	/// </summary>
	public string? MetadataJson { get; set; }

}
