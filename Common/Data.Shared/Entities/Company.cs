//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 1/27/2026 2:22 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------

namespace Dyvenix.App1.Data.Shared.Entities;

public partial class Company
{
	// PK
	public Guid Id { get; set; }

	// Properties
	public string Name { get; set; } = null!;
	public string Region { get; set; } = null!;
	public bool IsSystem { get; set; }

	#region PropNames

	public static class PropNames
	{
		public const string Id = "Id";
		public const string Name = "Name";
		public const string Region = "Region";
		public const string IsSystem = "IsSystem";
	}

	#endregion
}
