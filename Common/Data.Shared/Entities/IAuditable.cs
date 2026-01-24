namespace Dyvenix.App1.Data.Shared.Entities;

/// <summary>
/// Interface for entities that track creation and modification audit information.
/// </summary>
public interface IAuditable
{
    /// <summary>UTC timestamp when the entity was created.</summary>
    DateTime CreatedAt { get; set; }

    /// <summary>User ID who created the entity.</summary>
    Guid? CreatedBy { get; set; }

    /// <summary>UTC timestamp when the entity was last updated.</summary>
    DateTime? UpdatedAt { get; set; }

    /// <summary>User ID who last updated the entity.</summary>
    Guid? UpdatedBy { get; set; }
}
