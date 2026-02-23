using Microsoft.AspNetCore.Identity;

namespace OpeniddictServer.Data;

public class ApplicationUser : IdentityUser
{
    public Guid TenantId { get; set; }
}
