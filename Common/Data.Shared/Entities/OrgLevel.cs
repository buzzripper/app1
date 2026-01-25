namespace Dyvenix.App1.Data.Shared.Entities;

/// <summary>
/// Defines the hierarchical level of an organization unit within a tenant.
/// </summary>
public enum OrgLevel
{
    /// <summary>Corporate headquarters or top-level organization.</summary>
    Corporate = 0,

    /// <summary>Regional division (e.g., North America, EMEA).</summary>
    Region = 1,

    /// <summary>District or zone within a region.</summary>
    District = 2,

    /// <summary>Local office or branch location.</summary>
    Office = 3,

    /// <summary>Department within an office.</summary>
    Department = 4,

    /// <summary>Team within a department.</summary>
    Team = 5
}
