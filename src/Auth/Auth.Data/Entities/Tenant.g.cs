//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/28/2026 11:36 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Auth.Shared.DTOs;

namespace Dyvenix.App1.Auth.Data.Entities;

public partial class Tenant
{
	// PK
	public Guid Id { get; set; }

	// Properties
	public string Name { get; set; } = null!;
	public string Slug { get; set; } = null!;
	public AuthMode AuthMode { get; set; }
	public string? ExternalAuthority { get; set; }
	public string? ExternalClientId { get; set; }
	public string? ExternalClientSecret { get; set; }
	public string? ADDcHost { get; set; }
	public string? ADDomain { get; set; }
	public int? ADLdapPort { get; set; }
	public string? ADBaseDn { get; set; }
	public bool IsActive { get; set; }
	public DateTime CreatedAt { get; set; }

	#region PropNames

	public static class PropNames
	{
		public const string Id = "Id";
		public const string Name = "Name";
		public const string Slug = "Slug";
		public const string AuthMode = "AuthMode";
		public const string ExternalAuthority = "ExternalAuthority";
		public const string ExternalClientId = "ExternalClientId";
		public const string ExternalClientSecret = "ExternalClientSecret";
		public const string ADDcHost = "ADDcHost";
		public const string ADDomain = "ADDomain";
		public const string ADLdapPort = "ADLdapPort";
		public const string ADBaseDn = "ADBaseDn";
		public const string IsActive = "IsActive";
		public const string CreatedAt = "CreatedAt";
	}

	#endregion
}
