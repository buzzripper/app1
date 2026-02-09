//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/8/2026 8:50 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------

namespace Dyvenix.App1.Common.Data.Shared.Entities;

public partial class Patient
{
	// PK
	public Guid Id { get; set; }

	// Properties
	public string LastName { get; set; } = null!;
	public string FirstName { get; set; } = null!;
	public string? Email { get; set; }
	public bool IsActive { get; set; }

	// Navigation Properties
	public List<Invoice> Invoices { get; set; } = new();

	#region PropNames

	public static class PropNames
	{
		public const string Id = "Id";
		public const string FirstName = "FirstName";
		public const string LastName = "LastName";
		public const string Email = "Email";
		public const string IsActive = "IsActive";
		public const string Invoices = "Invoices";
	}

	#endregion
}
