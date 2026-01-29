//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 1/27/2026 12:24 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------

namespace Dyvenix.App1.Common.Data.Shared.Entities;

public partial class Invoice
{
	// PK
	public Guid Id { get; set; }

	// FKs
	public Guid PersonId { get; set; }

	// Properties
	public string InvoiceNum { get; set; } = null!;
	public string Amount { get; set; } = null!;
	public string IsPaid { get; set; } = null!;
	public string Description { get; set; } = null!;

	#region PropNames

	public static class PropNames
	{
		public const string Id = "Id";
		public const string InvoiceNum = "InvoiceNum";
		public const string Amount = "Amount";
		public const string IsPaid = "IsPaid";
		public const string Description = "Description";
		public const string PersonId = "PersonId";
	}

	#endregion
}
