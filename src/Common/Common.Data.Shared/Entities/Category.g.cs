//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/12/2026 8:04 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------

namespace Dyvenix.App1.Common.Data.Shared.Entities;

public partial class Category
{
	// PK
	public Guid Id { get; set; }

	// Properties
	public string Name { get; set; } = null!;

	// Navigation Properties
	public List<Invoice> Invoice { get; set; } = new();

	#region PropNames

	public static class PropNames
	{
		public const string Id = "Id";
		public const string Name = "Name";
		public const string Invoice = "Invoice";
	}

	#endregion
}
