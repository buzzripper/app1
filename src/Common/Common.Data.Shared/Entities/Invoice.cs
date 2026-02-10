//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/10/2026 11:50 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------

namespace Dyvenix.App1.Common.Data.Shared.Entities;

public partial class Invoice
{
	// PK
	public Guid Id { get; set; }

	// FKs
	public Guid PatientId { get; set; }
	public Guid CategoryId { get; set; }

	// Properties
	public decimal Amount { get; set; }
	public string Memo { get; set; } = null!;

	#region PropNames

	public static class PropNames
	{
		public const string Id = "Id";
		public const string PatientId = "PatientId";
		public const string Amount = "Amount";
		public const string Memo = "Memo";
		public const string CategoryId = "CategoryId";
	}

	#endregion
}
