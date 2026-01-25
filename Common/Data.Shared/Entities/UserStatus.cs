namespace Dyvenix.App1.Data.Shared.Entities;

/// <summary>
/// Defines the status of a user account.
/// </summary>
public enum UserStatus
{
    /// <summary>User has been invited but not yet verified.</summary>
    PendingVerification = 0,

    /// <summary>User account is active and can access the system.</summary>
    Active = 1,

    /// <summary>User account is temporarily suspended.</summary>
    Suspended = 2,

    /// <summary>User account is locked due to security reasons (e.g., failed login attempts).</summary>
    Locked = 3,

    /// <summary>User account has been deactivated.</summary>
    Inactive = 4
}
