using Microsoft.AspNetCore.Identity;

namespace Dyvenix.App1.Auth.Data;

public class ApplicationRole : IdentityRole
{
    public Guid TenantId { get; set; }
}
