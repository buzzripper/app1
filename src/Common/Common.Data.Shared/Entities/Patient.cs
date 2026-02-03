//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/3/2026 9:41 AM. Any changes made to it will be lost.
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

	// Navigation Properties
	public List<Invoice> Invoices { get; set; } = new();

	#region PropNames

	public static class PropNames
	{
		public const string Id = "Id";
		public const string FirstName = "FirstName";
		public const string LastName = "LastName";
		public const string Email = "Email";
		public const string Invoices = "Invoices";
	}

	#endregion
}
