namespace Dyvenix.App1.Data.Shared.Entities;

/// <summary>
/// Represents a user in the application.
/// Users belong to a single tenant but can be members of multiple organizations within that tenant.
/// </summary>
public class AppUser : BaseEntity
{
    /// <summary>Foreign key to the owning tenant.</summary>
    public Guid TenantId { get; set; }

    /// <summary>Navigation property to the owning tenant.</summary>
    public Tenant Tenant { get; set; } = null!;

    /// <summary>
    /// External identity provider user ID (e.g., Azure AD object ID, Auth0 user ID).
    /// Used to link the local user record to the external identity provider.
    /// </summary>
    public string? ExternalId { get; set; }

    /// <summary>User's email address (typically used as username).</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Indicates if the email address has been verified.</summary>
    public bool EmailVerified { get; set; }

    /// <summary>User's first name.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>User's last name.</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>User's display name (defaults to FirstName + LastName if not set).</summary>
    public string? DisplayName { get; set; }

    /// <summary>User's phone number.</summary>
    public string? Phone { get; set; }

    /// <summary>Indicates if the phone number has been verified.</summary>
    public bool PhoneVerified { get; set; }

    /// <summary>User's job title.</summary>
    public string? JobTitle { get; set; }

    /// <summary>User's department.</summary>
    public string? Department { get; set; }

    /// <summary>URL or path to the user's profile picture/avatar.</summary>
    public string? AvatarUrl { get; set; }

    /// <summary>Current status of the user account.</summary>
    public UserStatus Status { get; set; } = UserStatus.PendingVerification;

    /// <summary>IANA timezone identifier for the user's preferred timezone.</summary>
    public string TimeZone { get; set; } = "UTC";

    /// <summary>Locale/culture code for the user's preferred language (e.g., "en-US").</summary>
    public string Locale { get; set; } = "en-US";

    /// <summary>User-specific settings/preferences stored as JSON.</summary>
    public string? SettingsJson { get; set; }

    /// <summary>UTC timestamp of the user's last login.</summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>IP address of the user's last login.</summary>
    public string? LastLoginIp { get; set; }

    /// <summary>Number of consecutive failed login attempts.</summary>
    public int FailedLoginAttempts { get; set; }

    /// <summary>UTC timestamp when the account was locked due to failed login attempts.</summary>
    public DateTime? LockedUntil { get; set; }

    /// <summary>UTC timestamp when the user must change their password.</summary>
    public DateTime? PasswordExpiresAt { get; set; }

    /// <summary>Indicates if multi-factor authentication is enabled for this user.</summary>
    public bool MfaEnabled { get; set; }

    /// <summary>User ID who invited this user to the system.</summary>
    public Guid? InvitedBy { get; set; }

    /// <summary>UTC timestamp when the user was invited.</summary>
    public DateTime? InvitedAt { get; set; }

    /// <summary>UTC timestamp when the user accepted the invitation and completed registration.</summary>
    public DateTime? AcceptedInviteAt { get; set; }

    /// <summary>Foreign key to the user's primary organization.</summary>
    public Guid? PrimaryOrganizationId { get; set; }

    /// <summary>Navigation property to the user's primary organization.</summary>
    public Organization? PrimaryOrganization { get; set; }

    // Navigation properties

    /// <summary>Roles assigned to this user.</summary>
    public ICollection<Role> Roles { get; set; } = [];

    /// <summary>Permissions directly assigned to this user (in addition to role-based permissions).</summary>
    public ICollection<Permission> Permissions { get; set; } = [];

    /// <summary>Organization memberships for this user.</summary>
    public ICollection<UserOrganization> UserOrganizations { get; set; } = [];
}
