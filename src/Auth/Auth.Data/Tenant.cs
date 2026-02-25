namespace Dyvenix.App1.Auth.Data;

public class Tenant
{
	public Guid Id { get; set; }

	public required string Name { get; set; }

	public required string Slug { get; set; }

	/// <summary>
	/// Authentication method for this tenant.
	/// "Local" = ASP.NET Identity (username/password).
	/// "ExternalOidc" = federate to an external IdP.
	/// </summary>
	public required string AuthMethod { get; set; }

	// External IdP settings (only used when AuthMethod = "ExternalOidc")
	public string? ExternalAuthority { get; set; }
	public string? ExternalClientId { get; set; }
	public string? ExternalClientSecret { get; set; }

	public bool IsActive { get; set; } = true;

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
