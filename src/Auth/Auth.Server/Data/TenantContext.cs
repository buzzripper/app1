namespace Dyvenix.App1.Auth.Server.Data;

public interface ITenantContext
{
	Guid TenantId { get; }
	string? TenantSlug { get; }
	Tenant? Tenant { get; }

	void Set(Tenant tenant);
}

public class TenantContext : ITenantContext
{
	public Guid TenantId => Tenant?.Id ?? Guid.Empty;
	public string? TenantSlug => Tenant?.Slug;
	public Tenant? Tenant { get; private set; }

	public void Set(Tenant tenant) => Tenant = tenant;
}
