namespace Dyvenix.App1.Data.Shared.Entities;

/// <summary>
/// Interface for entities that support soft delete functionality.
/// </summary>
public interface ISoftDeletable
{
    /// <summary>Indicates if the entity has been soft deleted.</summary>
    bool IsDeleted { get; set; }

    /// <summary>UTC timestamp when the entity was soft deleted.</summary>
    DateTime? DeletedAt { get; set; }

    /// <summary>User ID who deleted the entity.</summary>
    Guid? DeletedBy { get; set; }
}
