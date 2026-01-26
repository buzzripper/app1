//------------------------------------------------------------------------------------------------------------
// This file was auto-generated. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
namespace Dyvenix.App1.Common.Core.Entities;

public partial class AccessClaim
{
	// PK
	public Guid Id { get; set; }

	public byte[] RowVersion { get; set; }

	// FKs
	public Guid AppUserId { get; set; }

	// Properties
	public string ClaimName { get; set; }
	public string ClaimValue { get; set; }



	public static class PropNames
	{
		public const string Id = nameof(AccessClaim.Id);
		public const string ClaimName = nameof(AccessClaim.ClaimName);
		public const string ClaimValue = nameof(AccessClaim.ClaimValue);
		public const string AppUserId = nameof(AccessClaim.AppUserId);
	}
}
