namespace Dyvenix.App1.Data.Shared.Entities;

/// <summary>
/// Represents a tenant (customer account) in the multi-tenant SaaS application.
/// This is the top-level billing and isolation boundary.
/// </summary>
public class Tenant : BaseEntity
{
    /// <summary>Display name of the tenant/company.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Unique identifier/slug for the tenant (used in URLs, subdomains).</summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>Primary contact email for the tenant.</summary>
    public string ContactEmail { get; set; } = string.Empty;

    /// <summary>Primary contact phone number.</summary>
    public string? ContactPhone { get; set; }

    /// <summary>Billing email address.</summary>
    public string? BillingEmail { get; set; }

    /// <summary>Subscription tier/plan identifier.</summary>
    public string? SubscriptionTier { get; set; }

    /// <summary>External subscription/billing system identifier (e.g., Stripe customer ID).</summary>
    public string? ExternalBillingId { get; set; }

    /// <summary>Indicates if the tenant account is active.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>UTC timestamp when the trial period ends (null if not on trial).</summary>
    public DateTime? TrialEndsAt { get; set; }

    /// <summary>UTC timestamp when the subscription started.</summary>
    public DateTime? SubscriptionStartedAt { get; set; }

    /// <summary>Maximum number of users allowed for this tenant (null for unlimited).</summary>
    public int? MaxUsers { get; set; }

    /// <summary>Tenant-specific settings stored as JSON.</summary>
    public string? SettingsJson { get; set; }

    /// <summary>IANA timezone identifier for the tenant's default timezone.</summary>
    public string TimeZone { get; set; } = "UTC";

    /// <summary>Locale/culture code (e.g., "en-US").</summary>
    public string Locale { get; set; } = "en-US";

    // Navigation properties

    /// <summary>Organizations belonging to this tenant.</summary>
    public ICollection<Organization> Organizations { get; set; } = [];

    /// <summary>Roles defined for this tenant.</summary>
    public ICollection<Role> Roles { get; set; } = [];
}
