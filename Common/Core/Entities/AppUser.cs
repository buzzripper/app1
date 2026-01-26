//------------------------------------------------------------------------------------------------------------
// This file was auto-generated. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
namespace Dyvenix.App1.Common.Core.Entities;

public partial class AppUser
{
	// PK
	public Guid Id { get; set; }

	public byte[] RowVersion { get; set; }

	// Properties
	public bool IsEnabled { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Email { get; set; }
	public string CompanyId { get; set; }
	public string ExtId { get; set; }
	public int? GroupCode { get; set; }
	public UserType UserType { get; set; }

	// Navigation Properties
	public virtual ICollection<AccessClaim> Claims { get; set; } = new List<AccessClaim>();


	public static class PropNames
	{
		public const string Id = nameof(AppUser.Id);
		public const string IsEnabled = nameof(AppUser.IsEnabled);
		public const string FirstName = nameof(AppUser.FirstName);
		public const string LastName = nameof(AppUser.LastName);
		public const string Email = nameof(AppUser.Email);
		public const string CompanyId = nameof(AppUser.CompanyId);
		public const string ExtId = nameof(AppUser.ExtId);
		public const string GroupCode = nameof(AppUser.GroupCode);
		public const string UserType = nameof(AppUser.UserType);
	}
}
