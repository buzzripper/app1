namespace Dyvenix.App1.Data.Shared.Entities;

/// <summary>
/// Base class for entities providing common audit and soft delete fields.
/// Implements IAuditable and ISoftDeletable for use with EF interceptors.
/// </summary>
public abstract class BaseEntity : IAuditable, ISoftDeletable
{
    /// <summary>Primary key.</summary>
    public Guid Id { get; set; }

    /// <summary>UTC timestamp when the entity was created.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>User ID who created the entity.</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>UTC timestamp when the entity was last updated.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>User ID who last updated the entity.</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Indicates if the entity has been soft deleted.</summary>
    public bool IsDeleted { get; set; }

    /// <summary>UTC timestamp when the entity was soft deleted.</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>User ID who deleted the entity.</summary>
    public Guid? DeletedBy { get; set; }
}
