//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/7/2026 3:16 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------

namespace Dyvenix.App1.Common.Data.Shared.Entities;

public partial class Invoice
{
	// PK
	public Guid Id { get; set; }

	// FKs
	public Guid PersonId { get; set; }

	// Properties
	public decimal Amount { get; set; }
	public string Memo { get; set; } = null!;

	#region PropNames

	public static class PropNames
	{
		public const string Id = "Id";
		public const string PersonId = "PersonId";
		public const string Amount = "Amount";
		public const string Memo = "Memo";
	}

	#endregion
}
