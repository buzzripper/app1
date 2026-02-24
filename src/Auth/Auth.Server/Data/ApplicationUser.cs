using Microsoft.AspNetCore.Identity;

namespace Dyvenix.App1.Auth.Server.Data;

public class ApplicationUser : IdentityUser
{
    public Guid TenantId { get; set; }
}
