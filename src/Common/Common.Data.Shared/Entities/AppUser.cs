//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 1/27/2026 10:51 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------

namespace Dyvenix.App1.Common.Data.Shared.Entities;

public partial class AppUser
{
	// PK
	public Guid Id { get; set; }

	// FKs
	public Guid CompanyId { get; set; }

	// Properties
	public string Username { get; set; } = null!;
	public string Email { get; set; } = null!;

	#region PropNames

	public static class PropNames
	{
		public const string Id = "Id";
		public const string CompanyId = "CompanyId";
		public const string Username = "Username";
		public const string Email = "Email";
	}

	#endregion
}
